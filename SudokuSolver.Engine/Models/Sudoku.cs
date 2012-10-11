using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Engine
{
    public class Sudoku
    {
        #region Members

        private int _Size = 0;
        private ICollection<Square> _Squares = null;
        private ICollection<Number> _Numbers = null;
        private IList<Group> _HorizontalTypeGroups = null;
        private IList<Group> _VerticalTypeGroups = null;
        private IList<Group> _SquareTypeGroups = null;
        private List<Hint> _Hints = new List<Hint>();
        private List<Availability> _Availabilities = new List<Availability>();
        private bool _Ready = false;
        private bool _AutoSolve = false;

        #endregion

        #region Events

        public event Square.SquareEventHandler SquareNumberChanged;
        public event Hint.FoundEventHandler HintFound;

        #endregion

        #region Properties

        public int SudokuId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

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
                // Validate first
                double squareRootOfValue = 0;

                try
                {
                    squareRootOfValue = Math.Sqrt(value);
                }
                catch (Exception)
                {
                    throw new Exception("Please enter a valid sudoku size");
                }

                // The square root of the size must be a round number
                if (squareRootOfValue != Math.Round(squareRootOfValue))
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
            get { return (int)Math.Sqrt(Size); }
        }

        /// <summary>
        /// Total number of squares (for size 9, it's 81)
        /// </summary>
        public int TotalSize
        {
            get { return Size * Size; }
        }

        public int SquaresLeft
        {
            get { return GetNumbers().Single(x => x.IsZero).Count; }
        }

        /// <summary>
        /// Gets all the squares of the sudoku
        /// Count of the list equals to the TotalSize property
        /// </summary>
        public IEnumerable<Square> GetSquares()
        {
            return _Squares;
        }

        /// <summary>
        /// Gets all the used squares of the sudoku
        /// </summary>
        public IEnumerable<Square> GetUsedSquares()
        {
            return GetSquares().Where(s => !s.IsAvailable);
        }

        /// <summary>
        /// Gets all numbers which can be used in sudoku, including zero.
        /// Count of the list equals to Size property of the sudoku + 1 (for size 9, it's 10)
        /// </summary>
        public IEnumerable<Number> GetNumbers()
        {
            return _Numbers;
        }

        /// <summary>
        /// Gets all usable numbers, except zero.
        /// Count of the list equals to Size property of the sudoku
        /// </summary>
        public IEnumerable<Number> GetNumbersExceptZero()
        {
            return GetNumbers().Where(n => !n.IsZero);
        }

        /// <summary>
        /// Gets horizontal type square groups
        /// </summary>
        public IEnumerable<Group> GetHorizontalTypeGroups()
        {
            return _HorizontalTypeGroups;
        }

        /// <summary>
        /// Gets vertical type square groups
        /// </summary>
        public IEnumerable<Group> GetVerticalTypeGroups()
        {
            return _VerticalTypeGroups;
        }

        /// <summary>
        /// Gets square type square groups
        /// </summary>
        public IEnumerable<Group> GetSquareTypeGroups()
        {
            return _SquareTypeGroups;
        }

        /// <summary>
        /// Gets the hints
        /// During UpdateSquare() method, when the solver finds a hint, it puts them to this list.
        /// When Solve() method called, this list will be checked and if the hint has still the same conditions, then the square will be update with the value.
        /// </summary>
        //public IEnumerable<Hint> GetHints() { return _Hints; }
        public List<Hint> GetHints() { return _Hints; }

        public IEnumerable<Availability> GetAvailabilities() { return _Availabilities; }

        public IEnumerable<Availability> GetUsedAvailabilities() { return _Availabilities.Where(x => !x.IsAvailable); }

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
                    var hasInvalidAssignType = GetSquares().Any(s => (s.AssignType == AssignTypes.User || s.AssignType == AssignTypes.Solver) && !s.IsAvailable);
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
                if (!Ready)
                    throw new Exception("AutoSolve cannot be set to true if it's not in Ready state");

                _AutoSolve = value;

                if (_AutoSolve)
                    Solve();
            }
        }


        public IEnumerable<GroupNumberAvailabilityContainer> GetGroupNumberAvailabilities()
        {
            var list = new List<GroupNumberAvailabilityContainer>();

            foreach (var g in GetSquareTypeGroups())
            {
                foreach (var n in GetNumbersExceptZero())
                {
                    list.Add(new GroupNumberAvailabilityContainer() { GroupId = g.Id, Number = n.Value, Count = g.GetAvailableSquaresForNumber(n).Count() });
                }
            }

            return list;
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
            this.Size = size;

            //All numbers; numbers will be created as the size of the sudoku (default 9 + zero value = 10)
            _Numbers = new List<Number>(this.Size + 1);
            _Numbers.Add(new Number(this, 0)); //Zero value
            for (int i = 1; i <= Size; i++)
                _Numbers.Add(new Number(this, i));

            //All square groups
            _HorizontalTypeGroups = new List<Group>(Size);
            _VerticalTypeGroups = new List<Group>(Size);
            _SquareTypeGroups = new List<Group>(Size);

            for (int i = 1; i <= Size; i++)
            {
                //Generate the groups
                var horizontalGroup = new Group(i, GroupTypes.Horizontal, this);
                var verticalGroup = new Group(i, GroupTypes.Vertical, this);
                var squareGroup = new Group(i, GroupTypes.Square, this);

                //Register the events
                horizontalGroup.HintFound += new Hint.FoundEventHandler(Hint_Found);
                verticalGroup.HintFound += new Hint.FoundEventHandler(Hint_Found);
                squareGroup.HintFound += new Hint.FoundEventHandler(Hint_Found);

                //Add to the lists
                _HorizontalTypeGroups.Add(horizontalGroup);
                _VerticalTypeGroups.Add(verticalGroup);
                _SquareTypeGroups.Add(squareGroup);
            }

            //All squares
            _Squares = new List<Square>(TotalSize);
            for (int i = 0; i < TotalSize; i++)
            {
                //Square id
                var squareId = i + 1;

                //Find the indexes of the current square's groups
                int horizontalGroupIndex = i / Size;
                int verticalGroupIndex = (i + 1) % Size == 0 ? Size - 1 : ((i + 1) % Size) - 1;
                int squareGroupIndex = (verticalGroupIndex / SquareRootOfSize) + ((horizontalGroupIndex / SquareRootOfSize) * SquareRootOfSize);

                //Get the groups
                var horizontalTypeGroup = _HorizontalTypeGroups[horizontalGroupIndex];
                var verticalTypeGroup = _VerticalTypeGroups[verticalGroupIndex];
                var squareTypeGroup = _SquareTypeGroups[squareGroupIndex];

                //Generate the squares
                var square = new Square(squareId, this, horizontalTypeGroup, verticalTypeGroup, squareTypeGroup);
                square.NumberChanged += new Square.SquareEventHandler(Square_NumberChanged);
                square.HintFound += new Hint.FoundEventHandler(Hint_Found);
                _Squares.Add(square);

                // Get the squares availabilities
                _Availabilities.AddRange(square.GetAvailabilities());
            }
        }

        public void UpdateSquare(int squareId, int value)
        {
            //Assign type
            var type = !Ready ? AssignTypes.Initial : AssignTypes.User;

            //Update
            UpdateSquare(squareId, value, type);
        }

        public void UpdateSquare(int squareId, int value, AssignTypes type)
        {
            //Get the square
            var square = GetSquares().Single(s => s.Id.Equals(squareId));

            //Get the number
            var number = GetNumbers().Single(n => n.Value.Equals(value));

            //Update
            UpdateSquare(square, number, type);
        }

        void UpdateSquare(Square square, Number number, AssignTypes type)
        {
            //Validations;
            //a. Square
            if (square == null)
                throw new ArgumentNullException("square", "Not a valid square id");

            //b. Number
            if (number == null)
                throw new ArgumentNullException("number", "Not a valid number");

            //c. Is it valid; The value cannot be changed if the sudoku is in Ready state and the square has a initial value
            if (Ready && square.AssignType == AssignTypes.Initial && !square.IsAvailable)
                throw new InvalidOperationException("Not a valid assignment, initial values can not be changed in Ready state");

            //d. Is it available; Checks the related squares in the related groups
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
            //Update hints!
            //Is it correct place to do this?
            //And how about zero case?
            if (!square.IsAvailable)
            {
                _Hints.RemoveAll(p => p.Square.Equals(square));
            }

            if (SquareNumberChanged != null)
                SquareNumberChanged(square);
        }

        /// <summary>
        /// When the Hint_Found event raises from a square or squareGroup, this method adds the hint to the list.
        /// These squares will be checked in Solve method
        /// </summary>
        /// <param name="hint"></param>
        void Hint_Found(Hint hint)
        {
            if (!_Hints.Any(p => p.Square.Equals(hint.Square)))
            {
                _Hints.Add(hint);

                if (HintFound != null)
                    HintFound(hint);
            }
        }

        public void ToggleReady()
        {
            Ready = !Ready;
        }

        public void ToggleAutoSolve()
        {
            AutoSolve = !AutoSolve;
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
            if (GetHints().Count() == 0)
                return;

            //Copy to a new list; since the Hints can take new values during this operation, it's not possible to use that one directly
            var hints = GetHints().ToList();

            foreach (var hint in hints)
            {
                if (hint.Type != HintTypes.None)
                {
                    UpdateSquare(hint.Square, hint.Number, AssignTypes.Solver);
                }
                hint.Type = HintTypes.None;
            }

            //Remove the processed ones
            _Hints.RemoveAll(s => s.Type.Equals(HintTypes.None));

            //Again; since it could spot more squares during this method, run it again
            //If there are no hint, it will quit anyway
            Solve();
        }

        public void Reset()
        {
            foreach (var s in _Squares)
            {
                if (s.AssignType == AssignTypes.User
                    || s.AssignType == AssignTypes.Solver
                    || (s.AssignType == AssignTypes.Initial && !Ready))
                {
                    UpdateSquare(s.Id, 0, AssignTypes.Initial);
                }
            }
        }

        #endregion
    }
}