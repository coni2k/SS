using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    public partial class Square
    {
        #region - Members -

        private ICollection<Group> squareGroups;
        private ICollection<SquareAvailability> availabilities;

        #endregion

        #region - Events -

        public delegate void SquareEventHandler(Square square);

        // TODO We will come back ye, mi friend!
        internal event Hint.FoundEventHandler HintFound;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the parent sudoku class
        /// </summary>
        public Sudoku Sudoku { get; private set; }

        /// <summary>
        /// Id of the square
        /// </summary>
        public int SquareId { get; private set; }

        /// <summary>
        /// Value of the square
        /// </summary>
        public SudokuNumber SudokuNumber { get; private set; }

        /// <summary>
        /// Get the assign type of the square
        /// </summary>
        public AssignTypes AssignType { get; internal set; }

        public Group SquareTypeGroup { get; private set; }
        public Group HorizantalTypeGroup { get; private set; }
        public Group VerticalTypeGroup { get; private set; }

        /// <summary>
        /// Gets the groups that this square assigned to
        /// </summary>
        internal IEnumerable<Group> SquareGroups
        {
            get
            {
                if (squareGroups == null)
                {
                    squareGroups = new Collection<Group>();
                    squareGroups.Add(SquareTypeGroup);
                    squareGroups.Add(HorizantalTypeGroup);
                    squareGroups.Add(VerticalTypeGroup);
                }

                return squareGroups;
            }
        }

        /// <summary>
        /// Gets whether the square is available or not; if the number of the square is ZERO, it's an available one
        /// </summary>
        public bool IsAvailable
        {
            get { return SudokuNumber.IsZero; }
        }

        /// <summary>
        /// Holds the list of available numbers
        /// IMPORTANT This doesn't mean that this square is available or not.
        /// Even if this square has a value, this list may contain other numbers as available.
        /// The availability will only be determined by looking whether the related squares have that number or not.
        /// </summary>
        public IEnumerable<SquareAvailability> Availabilities
        {
            get { return availabilities; }
        }

        /// <summary>
        /// Determines whether the square value or it's availabilities were updated since the last UpdateSquare method call.
        /// </summary>
        internal bool Updated { get; set; }

        #endregion

        #region - Constructors -

        internal Square(int id, Sudoku sudoku,  Group squareTypeGroup, Group horizantalTypeGroup, Group verticalTypeGroup)
        {
            SquareId = id;
            Sudoku = sudoku;
            SudokuNumber = sudoku.ZeroNumber; // Zero as initial value
            AssignType = AssignTypes.Initial;

            // Groups
            SquareTypeGroup = squareTypeGroup;
            HorizantalTypeGroup = horizantalTypeGroup;
            VerticalTypeGroup = verticalTypeGroup;

            // Available numbers; assign all numbers, except zero
            availabilities = new Collection<SquareAvailability>();
            foreach (var sudokuNumber in Sudoku.NumbersExceptZero)
                availabilities.Add(new SquareAvailability(this, sudokuNumber));
        }

        #endregion

        #region - Methods -

        internal void Update(SudokuNumber number, AssignTypes type)
        {
            //// d. Is it available; Checks the related squares in the related groups
            //if (!number.IsZero && SquareGroups.Any(squareGroup => squareGroup.Squares.Any(square => square.SudokuNumber.Equals(number))))
            //    throw new InvalidOperationException("Not a valid assignment, the number is already in use in one of the related groups");

            // Before setting the new number up, make the old one available again
            // Except if the old value is zero, then there is nothing to do.
            // Zero value is not used in availabilities list (always available).
            if (!SudokuNumber.IsZero)
            {
                // Set availabilities of the related squares
                foreach (var group in SquareGroups)
                {
                    foreach (var square in group.Squares)
                    {
                        // TODO This also calls Check(), which is not necessary in this case?!
                        if (Sudoku.UseSquareLevelMethod)
                            square.UpdateAvailability(SudokuNumber, group.GroupType, null);

                        if (Sudoku.UseGroupNumberLevelMethod)
                            foreach (var squareGroup in square.SquareGroups)
                                squareGroup.UpdateAvailability(SudokuNumber, square, true);
                    }
                }
            }

            // Get the groups that got affected from UpdateAvailability operation (for Method 2)
            var relatedGroups = SquareGroups.SelectMany(group => group.Squares.SelectMany(square => square.SquareGroups)).Distinct();

            // Assign the new number to this square & let the number know it (there is a cross reference)
            SudokuNumber = number;

            // Set the type
            AssignType = type;

            // After setting the new number up, make it unavailable
            if (!SudokuNumber.IsZero)
            {
                // Set availabilities of the related squares
                foreach (var group in SquareGroups)
                {
                    foreach (var square in group.Squares)
                    {
                        if (Sudoku.UseSquareLevelMethod)
                            square.UpdateAvailability(SudokuNumber, group.GroupType, this);

                        if (Sudoku.UseGroupNumberLevelMethod)
                            foreach (var squareGroup in square.SquareGroups)
                            squareGroup.UpdateAvailability(SudokuNumber, square, false);
                    }
                }
            }

            // Method 2; Check whether there is any group that has only one square left for any number
            foreach (var group in relatedGroups)
                group.CheckGroupNumberAvailabilities();

            Updated = true;
        }

        /// <summary>
        /// Updates the availability of the square
        /// The value of "source" parameter determines whether it's going to be available or not.
        /// If it's null, the number will become available for that type or the other way around.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <param name="source"></param>
        internal void UpdateAvailability(SudokuNumber number, GroupTypes type, Square source)
        {
            // If the old value is zero, then there is nothing to do.
            // Zero value is not used in availabilities list (always available).
            //if (number.IsZero)
            //    return;

            // Set the availability
            Availabilities.Single(availability => availability.Number.Equals(number)).UpdateAvailability(type, source);

            // Search for hint
            CheckSquareAvailabilities();
        }

        internal void CheckSquareAvailabilities()
        {
            Sudoku.CheckSquareAvailabilitiesCounter++;

            // If it's not available, nothing to check
            if (!IsAvailable)
                return;

            // TODO !!!

            if (Sudoku.Hints.Any(hint => hint.Square.Equals(this) && hint.Type == HintTypes.Square))
            {
                // Get the available numbers
                var list = Availabilities.Where(availability => availability.IsAvailable);

                // If there is only one number left in the list, then we found a new hint
                if (list.Count() != 1)
                {
                    System.Diagnostics.Debug.WriteLine("Square.Group_SquareAvailabilityChanged found a hint to be REMOVED - Id: {0} - Value: {1}", SquareId.ToString(), SudokuNumber.Value.ToString());
                }
            }
            else
            {
                // Get the available numbers
                var availabilities = Availabilities.Where(availability => availability.IsAvailable);

                // If there is only one number left in the list, then we found a new hint
                if (availabilities.Count() == 1)
                {
                    // TODO NEW HINT CODE HERE
                    // Update(item.Number., AssignTypes.Hint);
                    // Hint_SquareGroup = null;

                    // Get the item from the list
                    var availability = availabilities.Single();

                    if (HintFound != null)
                        HintFound(new Hint(this, null, availability.Number, HintTypes.Square));
                }
            }
        }

        public override string ToString()
        {
            return string.Format("SquareId: {0} - Number: {1}", SquareId, SudokuNumber.Value);
        }

        #endregion
    }
}