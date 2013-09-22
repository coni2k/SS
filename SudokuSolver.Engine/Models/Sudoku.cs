using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SudokuSolver.Engine
{
    public class Sudoku
    {
        #region - Members -

        public int ActionCounter { get; private set; }
        public int SearchSquareHintCounter { get; internal set; }
        public int SearchNumberHintCounter { get; internal set; }
        public bool DisplaySquareDetails { get; set; }
        public bool DisplaySquareHints { get; set; }
        public bool DisplaySquareAvailabilities { get; set; }
        public bool DisplayGroupNumberAvailabilities { get; set; }

        private ICollection<SudokuNumber> numbers = new HashSet<SudokuNumber>();
        private SudokuNumber zeroNumber;
        private ICollection<Square> squares = new HashSet<Square>();
        private ICollection<Group> groups = new HashSet<Group>();
        private List<SquareAvailability> squareAvailabilities = new List<SquareAvailability>();
        private List<GroupNumberAvailability> groupNumberAvailabilities = new List<GroupNumberAvailability>();
        private bool ready;
        private bool autoSolve;

        #endregion

        #region - Properties -

        public int SudokuId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Size of the sudoku; number of squares in one row, column or group
        /// </summary>
        public int Size { get; private set; }

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
            get { return groups.Where(group => group.GroupType == GroupType.Square); }
        }

        /// <summary>
        /// Gets horizontal type square groups
        /// </summary>
        public IEnumerable<Group> HorizontalTypeGroups
        {
            get { return groups.Where(group => group.GroupType == GroupType.Horizontal); }
        }

        /// <summary>
        /// Gets vertical type square groups
        /// </summary>
        public IEnumerable<Group> VerticalTypeGroups
        {
            get { return groups.Where(group => group.GroupType == GroupType.Vertical); }
        }

        /// <summary>
        /// Gets availabilites of the squares
        /// </summary>
        public IEnumerable<SquareAvailability> SquareAvailabilities
        {
            get { return squareAvailabilities; }
        }

        /// <summary>
        /// Gets all the 'updated' square availabilities
        /// </summary>
        public IEnumerable<SquareAvailability> UpdatedSquareAvailabilities
        {
            get { return squareAvailabilities.Where(availability => availability.Updated); }
        }

        /// <summary>
        /// Gets availabilities of the group numbers
        /// </summary>
        public IEnumerable<GroupNumberAvailability> GroupNumberAvailabilities
        {
            get { return groupNumberAvailabilities; }
        }

        /// <summary>
        /// Gets all the 'updated' availabilities of the group numbers
        /// </summary>
        public IEnumerable<GroupNumberAvailability> UpdatedGroupNumberAvailabilities
        {
            get { return groupNumberAvailabilities.Where(availability => availability.Updated); }
        }

        public IEnumerable<Square> SquareMethodHintContainers
        {
            get { return Squares.Where(square => square.ContainsSquareMethodHint); }
        }

        public IEnumerable<Square> SquareMethodHints
        {
            get { return Squares.Where(square => square.IsSquareMethodHint); }
        }

        public IEnumerable<Square> NumberMethodHints
        {
            get { return Squares.Where(square => square.IsNumberMethodHint); }
        }

        public IEnumerable<Square> Hints
        {
            get { return Squares.Where(square => square.IsSquareMethodHint || square.IsNumberMethodHint); }
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
                    var hasInvalidAssignType = Squares.Any(square => (square.AssignType == AssignType.User || square.AssignType == AssignType.Solver) && !square.IsAvailable);
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

        // TODO !
        public bool UseSquareMethod { get; set; }
        public bool UseNumberMethod { get; set; }

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
            // Settings
            //DisplaySquareDetails = true;
            //DisplaySquareHints = true;
            DisplaySquareAvailabilities = false;
            DisplayGroupNumberAvailabilities = false;

            // Validate; the square root of the size must be a round number
            var squareRootOfValue = Math.Sqrt(size);

            if (squareRootOfValue != Math.Round(squareRootOfValue))
                throw new ArgumentException("Please enter a valid sudoku size", "size");

            Size = size;

            // Solve methods
            UseSquareMethod = true;
            UseNumberMethod = true;

            // Numbers (default 9 + zero value = 10)
            numbers.Add(ZeroNumber); // Zero value
            for (int i = 1; i <= Size; i++)
                numbers.Add(new SudokuNumber(this, i));

            // Square groups
            for (int i = 1; i <= Size; i++)
            {
                // Generate the groups
                var squareGroup = new Group(this, i, GroupType.Square);
                var horizontalGroup = new Group(this, i, GroupType.Horizontal);
                var verticalGroup = new Group(this, i, GroupType.Vertical);

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
                var squareTypeGroup = groups.Single(group => group.Id == squareGroupId && group.GroupType == GroupType.Square);
                var horizontalTypeGroup = groups.Single(group => group.Id == horizontalGroupId && group.GroupType == GroupType.Horizontal);
                var verticalTypeGroup = groups.Single(group => group.Id == verticalGroupId && group.GroupType == GroupType.Vertical);

                // Generate the squares
                var square = new Square(squareId, this, squareTypeGroup, horizontalTypeGroup, verticalTypeGroup);

                // Add to the list
                squares.Add(square);

                // Copy square availabilities to sudoku level
                squareAvailabilities.AddRange(square.Availabilities);
            }

            // Register squares to square groups
            foreach (var group in Groups)
            {
                group.Squares = Squares.Where(square => square.Groups.Any(squareGroup => squareGroup.Equals(group)));

                // Copy group number availabilities to sudoku level
                if (group.GroupType == GroupType.Square)
                    groupNumberAvailabilities.AddRange(group.GroupNumbers.SelectMany(groupNumber => groupNumber.Availabilities));
            }
        }

        public void UpdateSquare(int squareId, int numberValue)
        {
            // Get the square
            var selectedSquare = Squares.SingleOrDefault(square => square.SquareId.Equals(squareId));

            // Get the number
            var selectedNumber = Numbers.SingleOrDefault(number => number.Value.Equals(numberValue));

            // Assign type
            var type = selectedNumber.IsZero
                ? AssignType.Initial
                : Ready ? AssignType.User
                : AssignType.Initial;

            // Validations;
            //a. Square
            if (selectedSquare == null)
                throw new ArgumentNullException("squareId", "Not a valid square id");

            // b. Number
            if (selectedNumber == null)
                throw new ArgumentNullException("numberValue", "Not a valid number");

            // Update
            UpdateSquare(selectedSquare, selectedNumber, type);
        }

        void UpdateSquare(Square selectedSquare, SudokuNumber selectedNumber, AssignType type)
        {
            // Validations;
            // a. Is it a valid assignment; The value cannot be changed if the sudoku is in Ready state and the square has a initial value
            if (Ready && selectedSquare.AssignType == AssignType.Initial && !selectedSquare.IsAvailable)
                throw new InvalidOperationException("Not a valid assignment, initial values can not be changed in Ready state");

            // b. Is the selected square a hint?
            // TODO THINK ABOUT THE ERROR MESSAGE - THE END USER MAY NOT KNOW THAT THIS WAS A HINT ?!
            if (selectedSquare.AssignType == AssignType.Hint && !selectedNumber.Equals(selectedSquare.SudokuNumber))
                throw new InvalidOperationException("Not a valid assignment, hint values cannot be changed");

            // b. Is it available; check the related squares with the same number
            // TODO Check this place
            if (!selectedNumber.IsZero)
            {
                var existingGroups = selectedSquare.Groups.Where(squareGroup => squareGroup.Squares.Any(square => square.SudokuNumber.Equals(selectedNumber)));

                if (existingGroups.Any())
                {
                    foreach (var group in existingGroups)
                    {
                        var existingSquares = group.Squares.Where(square => !square.Equals(selectedSquare) && square.SudokuNumber.Equals(selectedNumber));

                        if (existingSquares.Any())
                        {
                            foreach (var square in existingSquares)
                            {
                                if (!selectedSquare.IsAvailable && square.AssignType == AssignType.Hint)
                                {
                                    // Ok
                                }
                                else
                                {
                                    throw new InvalidOperationException("Not a valid assignment, the number is already in use in one of the related groups");
                                }
                            }
                        }
                    }
                }
            }

            // Reset the 'Updated' flags
            ResetUpdated();

            // Reset the action counter
            ActionCounter = 0;

            // Set the number and type
            if (selectedSquare.SudokuNumber == selectedNumber)
                selectedSquare.Update(type);
            else
            {
                if (selectedNumber.IsZero && selectedSquare.ContainsSquareMethodHint && selectedSquare.ValidateHintStatus)
                {
                    //var relatedHints = selectedSquare.RelatedSquareMethodHints;
                    //var relatedHintsAvailabilities = relatedHints.SelectMany(h => h.Availabilities.Where(a => a.SudokuNumber == selectedSquare.SudokuNumber));
                    //var allRelatedHintsAlsoBelongsToAnotherSquare = relatedHintsAvailabilities.All(a => a.Sources.Any(s => s != null && !s.Equals(selectedSquare)));

                    //var shouldStayAsHint = !relatedHints.Any() || allRelatedHintsAlsoBelongsToAnotherSquare;

                    //if (selectedSquare.ContainsSquareMethodHint && shouldStayAsHint)
                    //{
                    selectedSquare.Update(AssignType.Hint);
                    //}
                    //else
                    //{
                    //    selectedSquare.Update(selectedNumber, type);
                    //}
                }
                else
                {
                    selectedSquare.Update(selectedNumber, type);
                }
            }

            // Update the number's Updated
            selectedNumber.Updated = true;

            // Solve?
            if (AutoSolve)
                Solve();
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

        public void ToggleReady()
        {
            Ready = !Ready;
        }

        public void ToggleAutoSolve()
        {
            AutoSolve = !AutoSolve;
        }

        /// <summary>
        /// TODO Update this text!
        /// To solve the sudoku, this logic checks the spotted squares.
        /// These are the squares which found during UpdateSquare() operation.
        /// </summary>
        public void Solve()
        {
            if (!Ready)
                throw new InvalidOperationException("Sudoku cannot be Solved if it's not in Ready state");

            if (!Hints.Any())
                return;

            foreach (var square in Hints)
                square.Update(AssignType.Solver);

            // Recursive; it can find new hints meanwhile.
            // If no hints left, it will quit anyway
            Solve();
        }

        public void Reset()
        {
            foreach (var square in squares)
            {
                if (square.AssignType == AssignType.User
                    || square.AssignType == AssignType.Solver
                    || (square.AssignType == AssignType.Initial && !Ready))
                {
                    UpdateSquare(square, ZeroNumber, AssignType.Initial);
                }
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "SudokuId: {0} - Title: {1}", SudokuId, Title);
        }

        #endregion
    }
}