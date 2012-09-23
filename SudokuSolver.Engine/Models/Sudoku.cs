using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Engine
{
    public class Sudoku
    {
        #region Members

        private int _Size = 0;
        //private List<Square> _Squares = null;
        //private IList<Number> _Numbers = null;
        //private IList<Group> _HorizontalTypeGroups = null;
        //private IList<Group> _VerticalTypeGroups = null;
        //private IList<Group> _SquareTypeGroups = null;
        private List<Potential> _PotentialSquares = null;
        private bool _Ready = false;
        private bool _AutoSolve = false;

        #endregion

        #region Events

        //internal delegate void InitializedEventHandler();
        //internal event InitializedEventHandler Initialized;

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
        public IEnumerable<Square> Squares { get; private set; } // { return _Squares; } }

        /// <summary>
        /// Gets all the used squares of the sudoku
        /// </summary>
        public IEnumerable<Square> UsedSquares
        {
            get { return Squares.Where(s => !s.IsAvailable); }
        }

        /// <summary>
        /// Gets all numbers which can be used in sudoku, including zero.
        /// Count of the list equals to Size property of the sudoku + 1 (for size 9, it's 10)
        /// </summary>
        public IEnumerable<Number> Numbers { get; private set; }

        /// <summary>
        /// Gets all usable numbers, except zero.
        /// Count of the list equals to Size property of the sudoku
        /// </summary>
        public IEnumerable<Number> NumbersExceptZero
        {
            get { return Numbers.Where(n => !n.IsZero); }
        }

        //public IEnumerable<Number> AvailableNumbers
        //{
        //    get { return NumbersExceptZero.Where(n => n.IsAvailable); }
        //}

        /// <summary>
        /// Gets horizontal type square groups
        /// </summary>
        public IEnumerable<Group> HorizontalTypeGroups { get; private set; } // { return _HorizontalTypeGroups; } }

        /// <summary>
        /// Gets vertical type square groups
        /// </summary>
        public IEnumerable<Group> VerticalTypeGroups { get; private set; } //{ return _VerticalTypeGroups; } }

        /// <summary>
        /// Gets square type square groups
        /// </summary>
        public IEnumerable<Group> SquareTypeGroups { get; private set; } // { return _SquareTypeGroups; } }

        /// <summary>
        /// Gets the potential squares
        /// During UpdateSquare() method, when the solver finds a potential square, it puts them to this list.
        /// When Solve() method called, this list will be checked and if the potential square has still the same conditions, then the square will be update with the value.
        /// </summary>
        public IEnumerable<Potential> PotentialSquares { get { return _PotentialSquares; } }

        /// <summary>
        /// Determines the whether sudoku is ready to solve or not.
        /// On application level, there can (will) be different colors, according to the square has the initial value or set by user/solver.
        /// If the sudoku is not ready, on any square update, Assign Type will be set to Initial value and will be set to User/Solver if otherwise.
        /// </summary>
        public bool Ready
        {
            get { return _Ready; }
            set
            {
                //Validate: Cannot set to false, if there are any Square with User/Solver assign type
                if (value == false)
                {
                    var hasInvalidAssignType = Squares.Any(s => (s.AssignType == AssignTypes.User || s.AssignType == AssignTypes.Solver) && !s.IsAvailable);
                    if (hasInvalidAssignType)
                        throw new Exception("Ready cannot set to false again if there are any squares with User or Solver Assign Type");
                }

                _Ready = value;
            }
        }

        /// <summary>
        /// Gets whether the solver will try to solve the sudoku automatically or not
        /// </summary>
        public bool AutoSolve
        {
            get { return _AutoSolve; }
            set
            {
                if (!this.Ready)
                    throw new Exception("AutoSolve cannot be set to true if it's not in Ready state");

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
            var numbers = new List<Number>(this.Size + 1);
            numbers.Add(new Number(this, 0)); //Zero value
            for (int i = 1; i <= Size; i++)
                numbers.Add(new Number(this, i));
            this.Numbers = numbers;

            //All square groups
            var horizontalTypeGroups = new List<Group>(Size);
            var verticalTypeGroups = new List<Group>(Size);
            var squareTypeGroups = new List<Group>(Size);

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

                //Add to the lists
                horizontalTypeGroups.Add(horizontalGroup);
                verticalTypeGroups.Add(verticalGroup);
                squareTypeGroups.Add(squareGroup);
            }

            //Assign to the public properties
            this.HorizontalTypeGroups = horizontalTypeGroups;
            this.VerticalTypeGroups = verticalTypeGroups;
            this.SquareTypeGroups = squareTypeGroups;

            //All squares
            var squares = new List<Square>(TotalSize);
            for (int i = 0; i < TotalSize; i++)
            {
                //Square id
                var squareId = i + 1;

                //Find the indexes of the current square's groups
                int horizontalGroupIndex = i / Size;
                int verticalGroupIndex = (i + 1) % Size == 0 ? Size - 1 : ((i + 1) % Size) - 1;
                int squareGroupIndex = (verticalGroupIndex / SquareRootOfSize) + ((horizontalGroupIndex / SquareRootOfSize) * SquareRootOfSize);

                //Get the groups
                var horizontalTypeGroup = horizontalTypeGroups[horizontalGroupIndex];
                var verticalTypeGroup = verticalTypeGroups[verticalGroupIndex];
                var squareTypeGroup = squareTypeGroups[squareGroupIndex];

                //Generate the squares
                Square square = new Square(squareId, this, horizontalTypeGroup, verticalTypeGroup, squareTypeGroup);
                square.NumberChanged += new Square.SquareEventHandler(Square_NumberChanged);
                square.PotentialSquareFound += new Potential.FoundEventHandler(PotentialSquare_Found);
                squares.Add(square);

                //Add the squares to their own groups
                //horizontalTypeGroup.AddSquare(square);
                //verticalTypeGroup.AddSquare(square);
                //squareTypeGroup.AddSquare(square);
            }
            this.Squares = squares;

            //Potential squares
            _PotentialSquares = new List<Potential>();

            //// Initialization completed; raise the event
            //if (Initialized != null)
            //    Initialized();
        }

        public void UpdateSquare(int squareId, int value)
        {
            //Get the square
            var square = this.Squares.Single(s => s.Id.Equals(squareId));

            //Get the number
            var number = this.Numbers.Single(n => n.Value.Equals(value));

            //Assign type
            var type = !this.Ready ? AssignTypes.Initial : AssignTypes.User;

            //Update
            UpdateSquare(square, number, type);
        }

        void UpdateSquare(Square square, Number number, AssignTypes type)
        {
            //Validations;
            //a. Square
            if (square == null)
                throw new Exception("Not a valid square id");

            //b. Number
            if (number == null)
                throw new Exception("Not a valid number");

            //c. Is it available; Checks the related squares in the related groups
            if (!number.IsZero && square.SquareGroups.Any(sg => sg.Squares.Any(s => s.Number.Equals(number))))
                throw new Exception("Not a valid assignment, the number is already in use in one of the related groups");

            //Set the number and type
            square.Update(number, type);

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
            //Update potentials!
            //Is it correct place to do this?
            //And how about zero case?
            if (!square.IsAvailable)
            {
                if (_PotentialSquares.Any(p => p.Square.Id.Equals(square.Id)))
                    _PotentialSquares.RemoveAll(p => p.Square.Id.Equals(square.Id));
            }

            if (SquareNumberChanged != null)
                SquareNumberChanged(square);
        }

        /// <summary>
        /// When the PotentialSquare_Found event raises from a square or squareGroup, this method adds the potential square to the list.
        /// These squares will be checked in Solve method
        /// </summary>
        /// <param name="potential"></param>
        void PotentialSquare_Found(Potential potential)
        {
            if (!_PotentialSquares.Any(p => p.Square.Id.Equals(potential.Square.Id)))
            {
                _PotentialSquares.Add(potential);

                if (PotentialSquareFound != null)
                    PotentialSquareFound(potential);
            }
        }

        /// <summary>
        /// To solve the sudoku, this logic checks the spotted squares.
        /// These are the squares which found during UpdateSquare() operation.
        /// </summary>
        public void Solve()
        {
            if (!this.Ready)
                throw new InvalidOperationException("Sudoku cannot be Solved if it's not in Ready state");

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
                    //if (Square.ValidatePotential(potential.Square))
                    //if (Square.ValidatePotential(potential.Square))
                    //{
                        UpdateSquare(potential.Square, potential.Number, AssignTypes.Solver);
                    //}
                    //else
                    //{
                    //   Console.WriteLine("we have a false potential");
                    //}
                }
                else if (potential.PotentialType == PotentialTypes.Group)
                {
                    //To be able to validate, get the potential square from the group one more time
                    // Square potentialFromGroup = potential.SquareGroup.GetPotentialSquare(potential.Number);

                    //If there is one square and if it's the same square from the potential class, then it's valid
                    //TODO Maybe we can found a way to be sure that the square from potential is always correct! Then these can be avoided?!
                    // if (potentialFromGroup != null && potential.Square.Equals(potentialFromGroup))
                        UpdateSquare(potential.Square, potential.Number, AssignTypes.Solver);
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