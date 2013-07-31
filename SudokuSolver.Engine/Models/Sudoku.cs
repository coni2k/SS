
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    public partial class Sudoku
    {
        #region - Members -

        public static int CheckSquareAvailabilitiesCounter;
        public static int CheckGroupNumberAvailabilitiesCounter;

        private int size;
        private ICollection<SudokuNumber> numbers = new Collection<SudokuNumber>();
        private SudokuNumber zeroNumber;
        private ICollection<Square> squares = new Collection<Square>();
        private ICollection<Group> groups = new Collection<Group>();
        private List<Hint> hints = new List<Hint>();
        private List<Square.SquareAvailability> squareAvailabilities = new List<Square.SquareAvailability>();
        private List<Group.GroupNumber.GroupNumberAvailability> groupNumberAvailabilities = new List<Group.GroupNumber.GroupNumberAvailability>();
        private bool ready;
        private bool autoSolve;

        #endregion

        #region - Events -

        public event Hint.FoundEventHandler HintFound;

        #endregion

        #region - Properties -

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
                return size;
            }
            private set
            {
                // Validate; the square root of the size must be a round number
                var squareRootOfValue = Math.Sqrt(value);

                if (squareRootOfValue != Math.Round(squareRootOfValue))
                    throw new ArgumentException("Please enter a valid sudoku size", "Size");

                size = value;
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
            get { return Numbers.Single(number => number.IsZero).Count; }
        }

        /// <summary>
        /// Gets all numbers which can be used in sudoku, including zero.
        /// Count of the list equals to Size property of the sudoku + 1 (for size 9, it's 10)
        /// </summary>
        public IEnumerable<SudokuNumber> Numbers
        {
            get { return numbers; }
        }

        /// <summary>
        /// Gets all usable numbers, except zero.
        /// Count of the list equals to Size property of the sudoku
        /// </summary>
        public IEnumerable<SudokuNumber> NumbersExceptZero
        {
            get { return Numbers.Where(number => !number.IsZero); }
        }

        public IEnumerable<SudokuNumber> UpdatedNumbers
        {
            get { return Numbers.Where(number => number.Updated); }
        }

        /// <summary>
        /// Initial value of the squares
        /// REMARK: ZeroNumber is always in Updated mode
        /// </summary>
        public SudokuNumber ZeroNumber
        {
            get { return zeroNumber ?? (zeroNumber = new SudokuNumber(this, 0) { Updated = true }); }
        }

        /// <summary>
        /// Gets all the squares of the sudoku
        /// Count of the list equals to the TotalSize property
        /// </summary>
        public IEnumerable<Square> Squares
        {
            get { return squares; }
        }

        /// <summary>
        /// Gets all the used squares of the sudoku
        /// </summary>
        public IEnumerable<Square> UsedSquares
        {
            get { return Squares.Where(square => !square.IsAvailable); }
        }

        /// <summary>
        /// Gets all the 'updated' squares
        /// </summary>
        public IEnumerable<Square> UpdatedSquares
        {
            get { return Squares.Where(square => square.Updated); }
        }

        public IEnumerable<Group> Groups
        {
            get { return groups; }
        }

        /// <summary>
        /// Gets square type square groups
        /// </summary>
        public IEnumerable<Group> SquareTypeGroups
        {
            get { return groups.Where(group => group.GroupType == GroupTypes.Square); }
        }

        /// <summary>
        /// Gets horizontal type square groups
        /// </summary>
        public IEnumerable<Group> HorizontalTypeGroups
        {
            get { return groups.Where(group => group.GroupType == GroupTypes.Horizontal); }
        }

        /// <summary>
        /// Gets vertical type square groups
        /// </summary>
        public IEnumerable<Group> VerticalTypeGroups
        {
            get { return groups.Where(group => group.GroupType == GroupTypes.Vertical); }
        }

        /// <summary>
        /// Gets availabilites of the squares
        /// </summary>
        public IEnumerable<Square.SquareAvailability> SquareAvailabilities
        {
            get { return squareAvailabilities; }
        }

        /// <summary>
        /// Gets all the 'updated' square availabilities
        /// </summary>
        public IEnumerable<Square.SquareAvailability> UpdatedSquareAvailabilities
        {
            get { return squareAvailabilities.Where(availability => availability.Updated); }
        }

        /// <summary>
        /// Gets availabilities of the group numbers
        /// </summary>
        public IEnumerable<Group.GroupNumber.GroupNumberAvailability> GroupNumberAvailabilities
        {
            get { return groupNumberAvailabilities; }
        }

        /// <summary>
        /// Gets all the 'updated' availabilities of the group numbers
        /// </summary>
        public IEnumerable<Group.GroupNumber.GroupNumberAvailability> UpdatedGroupNumberAvailabilities
        {
            get { return groupNumberAvailabilities.Where(availability => availability.Updated); }
        }

        /// <summary>
        /// Gets the hints
        /// During UpdateSquare() method, when the solver finds a hint, it puts them to this list.
        /// When Solve() method called, this list will be checked and if the hint has still the same conditions, then the square will be update with the value.
        /// </summary>
        //public IEnumerable<Hint> GetHints() { return _Hints; }
        // Availabilitypublic List<Hint> GetHints() { return hints; }
        public List<Hint> Hints
        {
            get { return hints; }
        }

        /// <summary>
        /// Determines the whether sudoku is ready to solve or not.
        /// On application level, there can (will) be different colors, according to the square has the initial value or set by user/solver.
        /// If the sudoku is not ready, on any square update, Assign Type will be set to Initial value and will be set to User/Solver if otherwise.
        /// </summary>
        public bool Ready
        {
            get { return ready; }
            set
            {
                //Validate: Cannot set to false, if there are any Square with User/Solver assign type
                if (value == false)
                {
                    var hasInvalidAssignType = Squares.Any(square => (square.AssignType == AssignTypes.User || square.AssignType == AssignTypes.Solver) && !square.IsAvailable);
                    if (hasInvalidAssignType)
                        throw new InvalidOperationException("Ready cannot set to false again if there are any squares with User or Solver Assign Type");
                }

                ready = value;
            }
        }

        /// <summary>
        /// Gets whether the solver will try to solve the sudoku automatically or not
        /// </summary>
        public bool AutoSolve
        {
            get { return autoSolve; }
            set
            {
                if (!Ready)
                    throw new InvalidOperationException("AutoSolve cannot be set to true if it's not in Ready state");

                autoSolve = value;

                if (autoSolve)
                    Solve();
            }
        }

        public bool UseSquareLevelMethod { get; set; }
        public bool UseGroupNumberLevelMethod { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Default constructor with size 9
        /// </summary>
        public Sudoku()
        {
            Init(9);
        }

        public Sudoku(int size)
        {
            Init(size);
        }

        #endregion

        #region - Methods -

        void Init(int size)
        {
            Size = size;

            // Solve methods
            UseSquareLevelMethod = false;
            UseGroupNumberLevelMethod = true;

            // Numbers (default 9 + zero value = 10)
            numbers.Add(ZeroNumber); // Zero value
            for (int i = 1; i <= Size; i++)
                numbers.Add(new SudokuNumber(this, i));

            // Square groups
            for (int i = 1; i <= Size; i++)
            {
                // Generate the groups
                var squareGroup = new Group(this, i, GroupTypes.Square);
                var horizontalGroup = new Group(this, i, GroupTypes.Horizontal);
                var verticalGroup = new Group(this, i, GroupTypes.Vertical);

                // Register the events
                squareGroup.HintFound += new Hint.FoundEventHandler(Hint_Found);
                horizontalGroup.HintFound += new Hint.FoundEventHandler(Hint_Found);
                verticalGroup.HintFound += new Hint.FoundEventHandler(Hint_Found);

                // Add to the lists
                groups.Add(squareGroup);
                groups.Add(horizontalGroup);
                groups.Add(verticalGroup);
            }

            // Squares
            for (int squareId = 1; squareId <= TotalSize; squareId++)
            {
                // Find the IDs of the current square's groups
                // TODO Try to add more info about these calculations

                // Square type
                int squareGroupId = (int)Decimal.Ceiling(Decimal.Divide(squareId, Size));

                // Horizontal group id calculation
                var h1 = (int)Decimal.Ceiling(Decimal.Divide(squareId, Size * SquareRootOfSize)) - 1;
                var h2 = (int)Decimal.Ceiling(Decimal.Divide(squareId, SquareRootOfSize));
                var h3 = h2 % SquareRootOfSize == 0 ? SquareRootOfSize : h2 % SquareRootOfSize;

                var horizontalGroupId = h3 + (h1 * SquareRootOfSize);

                // Vertical group id calculation
                var v1 = squareId % SquareRootOfSize == 0 ? SquareRootOfSize : squareId % SquareRootOfSize;
                var v2 = (int)Decimal.Ceiling(Decimal.Divide(squareId, Size));
                var v3 = (v2 - 1) % SquareRootOfSize;

                var verticalGroupId = v1 + (v3 * SquareRootOfSize);

                // Find the groups of the square
                var squareTypeGroup = groups.Single(group => group.Id == squareGroupId && group.GroupType == GroupTypes.Square);
                var horizontalTypeGroup = groups.Single(group => group.Id == horizontalGroupId && group.GroupType == GroupTypes.Horizontal);
                var verticalTypeGroup = groups.Single(group => group.Id == verticalGroupId && group.GroupType == GroupTypes.Vertical);

                // Generate the squares
                var square = new Square(squareId, this, squareTypeGroup, horizontalTypeGroup, verticalTypeGroup);

                // Register the event
                square.HintFound += new Hint.FoundEventHandler(Hint_Found);

                // Add to the list
                squares.Add(square);

                // Copy square availabilities to sudoku level
                squareAvailabilities.AddRange(square.Availabilities);
            }

            // Register squares to square groups
            foreach (var group in Groups)
            {
                group.Squares = Squares.Where(square => square.SquareGroups.Any(squareGroup => squareGroup.Equals(group)));

                // Copy group number availabilities to sudoku level
                groupNumberAvailabilities.AddRange(group.GroupNumbers.SelectMany(groupNumber => groupNumber.Availabilities));
            }
        }

        public void UpdateSquare(int squareId, int value)
        {
            // Assign type
            var type = !Ready ? AssignTypes.Initial : AssignTypes.User;

            // Update
            UpdateSquare(squareId, value, type);
        }

        void UpdateSquare(int squareId, int value, AssignTypes type)
        {
            // Get the square
            var selectedSquare = Squares.Single(square => square.SquareId.Equals(squareId));

            // Get the number
            var newNumber = Numbers.Single(number => number.Value.Equals(value));

            // Update
            UpdateSquare(selectedSquare, newNumber, type);
        }

        void UpdateSquare(Square selectedSquare, SudokuNumber newNumber, AssignTypes type)
        {
            CheckSquareAvailabilitiesCounter = 0;
            CheckGroupNumberAvailabilitiesCounter = 0;

            // Validations;
            //a. Square
            if (selectedSquare == null)
                throw new ArgumentNullException("square", "Not a valid square id");

            // b. Number
            if (newNumber == null)
                throw new ArgumentNullException("number", "Not a valid number");

            //// First remove the 
            //if (!selectedSquare.SudokuNumber.IsZero)
            //    selectedSquare.Update(ZeroNumber, selectedSquare.AssignType);

            // Set the number and type
            // selectedSquare.Update(newNumber, type);

            // c. Is it valid; The value cannot be changed if the sudoku is in Ready state and the square has a initial value
            if (Ready && selectedSquare.AssignType == AssignTypes.Initial && !selectedSquare.IsAvailable)
                throw new InvalidOperationException("Not a valid assignment, initial values can not be changed in Ready state");

            // d. Is it available; Checks the related squares in the related groups
            if (!newNumber.IsZero && selectedSquare.SquareGroups.Any(squareGroup => squareGroup.Squares.Any(square => square.SudokuNumber.Equals(newNumber))))
                throw new InvalidOperationException("Not a valid assignment, the number is already in use in one of the related groups");

            // Reset the 'Updated' flag
            ResetUpdated();

            // Set the number and type
            selectedSquare.Update(newNumber, type);

            // Update the number's Updated
            newNumber.Updated = true;

            // Solve?
            if (AutoSolve)
                Solve();
            // Solve(AutoSolve ? AssignTypes.Solver : AssignTypes.Hint );

            Console.WriteLine("CheckSquareAvailabilitiesCounter      : {0}", Sudoku.CheckSquareAvailabilitiesCounter);
            Console.WriteLine("CheckGroupNumberAvailabilitiesCounter : {0}", Sudoku.CheckGroupNumberAvailabilitiesCounter);
        }

        /// <summary>
        /// Set Updated properties to false before the next UpdateSquare method call.
        /// </summary>
        void ResetUpdated()
        {
            // Numbers, except Zero (it's always in Updated mode)
            foreach (var number in UpdatedNumbers.Where(number => number != ZeroNumber))
                number.Updated = false;

            // Squares
            foreach (var square in UpdatedSquares)
                square.Updated = false;

            // Square availabilities
            foreach (var availability in UpdatedSquareAvailabilities)
                availability.Updated = false;

            // Group number availabilities
            foreach (var availability in UpdatedGroupNumberAvailabilities)
                availability.Updated = false;
        }

        /// <summary>
        /// When the Hint_Found event raises from a square or squareGroup, this method adds the hint to the list.
        /// These squares will be checked in Solve method
        /// </summary>
        /// <param name="hint"></param>
        void Hint_Found(Hint hint)
        {
            if (!hints.Any(p => p.Square.Equals(hint.Square)))
            {
                hints.Add(hint);

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

        //public void Solve()
        //{
        //    Solve(AssignTypes.Solver);
        //}

        /// <summary>
        /// To solve the sudoku, this logic checks the spotted squares.
        /// These are the squares which found during UpdateSquare() operation.
        /// </summary>
        public void Solve()
        {
            if (!Ready)
                throw new InvalidOperationException("Sudoku cannot be Solved if it's not in Ready state");

            // Is there anything to do?
            if (Hints.Count() == 0)
                return;

            // Copy to a new list; since the Hints can take new values during this operation, it's not possible to use that one directly
            var hintsCopy = Hints.ToList();

            foreach (var hint in hintsCopy)
            {
                if (hint.Type != HintTypes.None)
                {
                    UpdateSquare(hint.Square, hint.Number, AssignTypes.Solver);
                }
                hint.Type = HintTypes.None;
            }

            // Remove the processed ones
            hintsCopy.RemoveAll(s => s.Type.Equals(HintTypes.None));

            // Again; since it could spot more squares during this method, run it again
            // If there are no hint, it will quit anyway
            // Solve(assignType);
            Solve();
        }

        public void Reset()
        {
            foreach (var square in squares)
            {
                if (square.AssignType == AssignTypes.User
                    || square.AssignType == AssignTypes.Solver
                    || (square.AssignType == AssignTypes.Initial && !Ready))
                {
                    UpdateSquare(square.SquareId, 0, AssignTypes.Initial);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("SudokuId: {0} - Title: {1}", SudokuId, Title);
        }

        #endregion
    }
}