using System;
using System.Collections.Generic;
using System.Linq;

namespace OSP.SudokuSolver.Engine
{
    public class Sudoku
    {
        #region Members

        private int _Size = 0;
        private IList<Square> _AllSquares = null;
        private IList<Number> _AllNumbers = null;
        private IList<Group> _AllHorizontalTypeGroups = null;
        private IList<Group> _AllVerticalTypeGroups = null;
        private IList<Group> _AllSquareTypeGroups = null;
        private List<Potential> _PotentialSquares = null;
        private bool _AutoSolve = false;

        #endregion

        #region Events

        public event Square.SquareEventHandler SquareNumberChanged;
        public event Potential.FoundEventHandler PotentialSquareFound;

        #endregion

        #region Properties

        /// <summary>
        /// Id of the sudoku
        /// TODO Actually this is nothing to do with the engine, it's more about data/database
        /// Maybe there can be SudokuData project and SudokuContainer class, which can hold this class and the ID as a seperate property.
        /// </summary>
        /* public Guid Id { get; private set; } */
        //public int Id { get; set; }

        /// <summary>
        /// Size of the sudoku; number of squares in one row, column or group
        /// </summary>
        public int Size
        {
            get
            {
                return _Size;
            }
            private set
            {
                //Validate first
                double squareRootOfValue = 0;

                //TODO Is there any chance to have an exception over here?
                try
                {
                    squareRootOfValue = Math.Sqrt(value);
                }
                catch (Exception)
                {
                    throw new Exception("Please enter a valid sudoku size");
                }

                //The square root of the size must be a round number
                if (!squareRootOfValue.Equals(Math.Round(squareRootOfValue)))
                    throw new Exception("Please enter a valid sudoku size");

                _Size = value;
            }
        }

        /// <summary>
        /// Square root of the sudoku size (for size 9, it's 3)
        /// </summary>
        /// <returns></returns>
        public int SquareRootOfSize
        {
            get { return (int)Math.Sqrt(this.Size); }
        }

        /// <summary>
        /// Total number of squares (for size 9, it's 81)
        /// </summary>
        public int TotalSize
        {
            get { return Size * Size; }
        }

        /// <summary>
        /// Gets all the squares of the sudoku
        /// Count of the list equals to the TotalSize property
        /// </summary>
        public IEnumerable<Square> AllSquares { get { return _AllSquares; } }

        /// <summary>
        /// Gets all the filled squares of the sudoku
        /// </summary>
        public IEnumerable<Square> AllFilledSquares
        {
            get { return AllSquares.Where(s => !s.IsAvailable); }
        }

        /// <summary>
        /// Gets all usable numbers, including zero.
        /// Count of the list equals to Size property of the sudoku + 1 (for size 9, it's 10)
        /// </summary>
        public IEnumerable<Number> AllNumbers { get { return _AllNumbers; } }

        /// <summary>
        /// Gets all usable numbers, except zero.
        /// Count of the list equals to Size property of the sudoku
        /// </summary>
        public IEnumerable<Number> AllNumbersExceptZero
        {
            get { return AllNumbers.Where(n => !n.IsZero); }
        }

        public IEnumerable<Number> AllAvailableNumbers
        {
            get { return AllNumbersExceptZero.Where(n => n.IsAvailable); }
        }

        /// <summary>
        /// Gets horizontal type square groups
        /// </summary>
        public IEnumerable<Group> AllHorizontalTypeGroups { get { return _AllHorizontalTypeGroups; } }

        /// <summary>
        /// Gets vertical type square groups
        /// </summary>
        public IEnumerable<Group> AllVerticalTypeGroups { get { return _AllVerticalTypeGroups; } }

        /// <summary>
        /// Gets square type square groups
        /// </summary>
        public IEnumerable<Group> AllSquareTypeGroups { get { return _AllSquareTypeGroups; } }

        /// <summary>
        /// Gets the potential squares
        /// During FillSquare() method, when the solver finds a potential square, it puts them to this list.
        /// When Solve() method called, this list will be checked and if the potential square has still the same conditions, then the square will be filled the value.
        /// </summary>
        public IEnumerable<Potential> PotentialSquares { get { return _PotentialSquares; } }

        /// <summary>
        /// Gets whether the solver will try to solve the sudoku automatically or not
        /// </summary>
        public bool AutoSolve
        {
            get { return _AutoSolve; }
            set
            {
                _AutoSolve = value;

                if (_AutoSolve)
                    Solve();
            }
        }

        #endregion

        #region Constructors

        public Sudoku()
        {
            //Default sudoku size is 9
            Init(9);
        }

        public Sudoku(int size)
        {
            Init(size);
        }

        #endregion

        #region Methods

        void Init(int size)
        {
            //Id
            //this.Id = Guid.NewGuid();

            //Size
            this.Size = size;

            //All numbers; numbers will be created as the size of the sudoku (default 9 + zero value = 10)
            _AllNumbers = new List<Number>(Size + 1);

            //Zero first
            var zeroNumber = new Number(this, 0);
            _AllNumbers.Add(zeroNumber);

            //Other numbers
            for (int i = 1; i <= Size; i++)
            {
                var number = new Number(this, i);
                _AllNumbers.Add(number);
            }

            //All square groups
            _AllHorizontalTypeGroups = new List<Group>(Size);
            _AllVerticalTypeGroups = new List<Group>(Size);
            _AllSquareTypeGroups = new List<Group>(Size);

            for (int i = 1; i <= Size; i++)
            {
                //Generate the groups
                var horizontalGroup = new Group(i, GroupTypes.Horizontal, this);
                var verticalGroup = new Group(i, GroupTypes.Vertical, this);
                var squareGroup = new Group(i, GroupTypes.Square, this);

                //Register the events
                horizontalGroup.PotentialFound += new Potential.FoundEventHandler(PotentialSquare_Found);
                verticalGroup.PotentialFound += new Potential.FoundEventHandler(PotentialSquare_Found);
                squareGroup.PotentialFound += new Potential.FoundEventHandler(PotentialSquare_Found);

                //Add to the "all" lists
                _AllHorizontalTypeGroups.Add(horizontalGroup);
                _AllVerticalTypeGroups.Add(verticalGroup);
                _AllSquareTypeGroups.Add(squareGroup);
            }

            //All squares
            _AllSquares = new List<Square>(TotalSize);
            for (int i = 0; i < TotalSize; i++)
            {
                //Find the indexes of the current square's groups
                int horizontalGroupIndex = i / Size;
                int verticalGroupIndex = (i + 1) % Size == 0 ? Size - 1 : ((i + 1) % Size) - 1;
                int squareGroupIndex = (verticalGroupIndex / SquareRootOfSize) + ((horizontalGroupIndex / SquareRootOfSize) * SquareRootOfSize);

                //Get the groups
                var currentSquareHorizontalGroup = _AllHorizontalTypeGroups[horizontalGroupIndex];
                var currentSquareVerticalGroup = _AllVerticalTypeGroups[verticalGroupIndex];
                var currentSquareSquareGroup = _AllSquareTypeGroups[squareGroupIndex];

                //Generate the squares
                Square square = new Square(i + 1, zeroNumber, this, currentSquareHorizontalGroup, currentSquareVerticalGroup, currentSquareSquareGroup);
                square.NumberChanged += new Square.SquareEventHandler(Square_NumberChanged);
                square.PotentialSquareFound += new Potential.FoundEventHandler(PotentialSquare_Found);
                _AllSquares.Add(square);

                //Add the squares to their own groups
                currentSquareHorizontalGroup.AddSquare(square);
                currentSquareVerticalGroup.AddSquare(square);
                currentSquareSquareGroup.AddSquare(square);
            }

            //Potential squares
            _PotentialSquares = new List<Potential>();
        }

        public void FillSquare(int squareId, int value)
        {
            //Get the square
            var square = this.AllSquares.Where(s => s.Id.Equals(squareId)).FirstOrDefault();

            //Get the number
            var number = this.AllNumbers.Where(n => n.Value.Equals(value)).FirstOrDefault();

            //Fill
            FillSquare(square, number, FillTypes.User);
        }

        void FillSquare(Square square, Number number, FillTypes type)
        {
            //Validate first
            if (square == null)
                throw new Exception("Not a valid square id");

            if (number == null)
                throw new Exception("Not a valid number");

            //Set the number and type
            square.Fill(number, type);

            //Solve?
            if (AutoSolve)
                Solve();
        }

        /// <summary>
        /// Handles number changed event of the square; raises an event and makes the number of the square unavailable in the related squares
        /// </summary>
        /// <param name="square"></param>
        void Square_NumberChanged(Square square)
        {
            if (SquareNumberChanged != null)
                SquareNumberChanged(square);
        }

        /// <summary>
        /// When the PotentialSquare_Found event raises from a square or squareGroup, this method adds the potential square to the list.
        /// These squares will be checked in Solve method
        /// </summary>
        /// <param name="square"></param>
        void PotentialSquare_Found(Potential square)
        {
            if (!_PotentialSquares.Exists(p => p.Square.Id.Equals(square.Square.Id)))
            {
                _PotentialSquares.Add(square);

                if (PotentialSquareFound != null)
                    PotentialSquareFound(square);
            }
        }

        /// <summary>
        /// To solve the sudoku, this logic checks the spotted squares.
        /// These are the squares which found during FillSquare() operation.
        /// </summary>
        public void Solve()
        {
            //Is there anything to do?
            if (PotentialSquares.Count().Equals(0))
                return;

            //Copy to a new list; since the PotentialSquares can take new values during this operation, it's not possible to use that one directly
            var potentials = PotentialSquares.ToList();

            foreach (var potential in potentials)
            {
                if (potential.PotentialType == PotentialTypes.Square)
                {
                    //If it's still valid..
                    if (Square.ValidatePotential(potential.Square))
                        FillSquare(potential.Square, potential.Number, FillTypes.Solver);
                }
                else if (potential.PotentialType == PotentialTypes.Group)
                {
                    //To be able to validate, get the potential square from the group one more time
                    Square potentialFromGroup = potential.SquareGroup.GetPotentialSquare(potential.Number);

                    //If there is one square and if it's the same square from the potential class, then it's valid
                    //TODO Maybe we can found a way to be sure that the square from potential is always correct! Then these can be avoided?!
                    if (potentialFromGroup != null && potential.Square.Equals(potentialFromGroup))
                        FillSquare(potential.Square, potential.Number, FillTypes.Solver);
                }

                potential.PotentialType = PotentialTypes.None;
            }

            //Remove the processed ones?
            _PotentialSquares.RemoveAll(s => s.PotentialType.Equals(PotentialTypes.None));

            //Again; since it could spot more squares during this method, run it again
            //If there are no potential square, it will quit anyway
            Solve();
        }

        #endregion
    }
}