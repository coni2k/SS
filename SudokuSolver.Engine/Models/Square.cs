using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    public partial class Square
    {
        #region - Members -

        ICollection<Group> groups;
        ICollection<SquareAvailability> availabilities;
        ICollection<IHintNew> hintList;

        #endregion

        #region - Events -

        // public delegate void SquareEventHandler(Square square);

        // TODO We will come back ye, mi friend!
        // internal event Hint.FoundEventHandler HintFound;

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
        internal IEnumerable<Group> Groups
        {
            get
            {
                if (groups == null)
                {
                    groups = new Collection<Group>();
                    groups.Add(SquareTypeGroup);
                    groups.Add(HorizantalTypeGroup);
                    groups.Add(VerticalTypeGroup);
                }

                return groups;
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

        public IEnumerable<IHintNew> HintList
        {
            get { return hintList; }
        }

        /// <summary>
        /// Determines whether the square value or it's availabilities were updated since the last UpdateSquare method call.
        /// </summary>
        internal bool Updated { get; set; }

        #endregion

        #region - Constructors -

        internal Square(int id, Sudoku sudoku, Group squareTypeGroup, Group horizantalTypeGroup, Group verticalTypeGroup)
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
            // a. Clear availabilities
            // Make the old number available again
            UpdateAvailabilities(false);

            // b. Remove hints
            // TODO!

            // Assign the new number
            SudokuNumber = number;

            // Set the type
            AssignType = type;

            // c. Update availabilities
            // Make the new number unavailable in the related square / group numbers
            UpdateAvailabilities(true);

            Updated = true;

            // d. Search hints
            // Square level method
            if (Sudoku.UseSquareLevelMethod)
            {
                var relatedSquares = groups.SelectMany(group => group.Squares).Distinct();
                foreach (var square in relatedSquares)
                    square.SearchSquareHint();
            }

            // Group level method
            if (Sudoku.UseGroupNumberLevelMethod)
            {
                var relatedGroups = Groups.SelectMany(group => group.Squares.SelectMany(square => square.Groups)).Distinct();
                foreach (var group in relatedGroups)
                    group.SearchGroupNumberHint();
            }
        }

        /// <summary>
        /// Updates the availabilities in related squares and group numbers
        /// </summary>
        void UpdateAvailabilities(bool isNumberUpdated)
        {
            // Ignore zero number, it's not used in availability lists (always available)
            if (SudokuNumber.IsZero)
                return;

            /* isNumberUpdated is false : Make the old number available
             *                 is true  : new number assigned to the square - make it unavailable in the related object. */
            var sourceSquare = isNumberUpdated ? this : null;
            var isGroupNumbersAvailable = !isNumberUpdated;

            // Set availabilities of the related squares
            foreach (var group in Groups)
            {
                foreach (var square in group.Squares)
                {
                    if (Sudoku.UseSquareLevelMethod)
                        square.UpdateAvailability(SudokuNumber, group.GroupType, sourceSquare);

                    if (Sudoku.UseGroupNumberLevelMethod)
                    {
                        foreach (var squareGroup in square.Groups)
                            squareGroup.UpdateAvailability(SudokuNumber, square, isGroupNumbersAvailable);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the availability of the square
        /// If there is "source", it means it's not available.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <param name="source"></param>
        internal void UpdateAvailability(SudokuNumber number, GroupTypes type, Square source)
        {
            Availabilities.Single(availability => availability.Number.Equals(number)).UpdateAvailability(type, source);
        }

        internal void SearchSquareHint()
        {
            Sudoku.SearchSquareHintCounter++;

            var lastAvailability = Availabilities.IfSingleOrDefault(availability => availability.IsAvailable);

            if (lastAvailability == null)
                return;

            if (!lastAvailability.Square.IsAvailable)
                return;
            
            if (Sudoku.Hints.Any(hint => hint.Square.Equals(lastAvailability.Square)))
                return;

            // Old version
            Sudoku.Hints.Add(new Hint(this, null, lastAvailability.Number, HintTypes.Square));

            // New version
            //hintList.Add((IHintNew)squareHint);            
        }

        //void ClearSquareHint()
        //{
        //    var count = Availabilities.Count(availability => availability.IsAvailable);

        //    if (count > 1 && hintList.Any(hint => hint.GetType().IsAssignableFrom(typeof(SquareHint)) && ((SquareHint) hint).Equals(this)))
        //        hintList.

        //}

        //internal void SearchSquareHintOld()
        //{
        //    Sudoku.CheckSquareAvailabilitiesCounter++;

        //    // If it's not available, nothing to check
        //    if (!IsAvailable)
        //        return;

        //    // TODO !!!

        //    if (Sudoku.Hints.Any(hint => hint.Square.Equals(this) && hint.Type == HintTypes.Square))
        //    {
        //        // Get the available numbers
        //        var list = Availabilities.Where(availability => availability.IsAvailable);

        //        // If there is only one number left in the list, then we found a new hint
        //        if (list.Count() != 1)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Square.Group_SquareAvailabilityChanged found a hint to be REMOVED - Id: {0} - Value: {1}", SquareId.ToString(), SudokuNumber.Value.ToString());
        //        }
        //    }
        //    else
        //    {
        //        // Get the available numbers
        //        var availabilities = Availabilities.Where(availability => availability.IsAvailable);

        //        // If there is only one number left in the list, then we found a new hint
        //        if (availabilities.Count() == 1)
        //        {
        //            // TODO NEW HINT CODE HERE
        //            // Update(item.Number., AssignTypes.Hint);
        //            // Hint_SquareGroup = null;

        //            // Get the item from the list
        //            var availability = availabilities.Single();

        //            if (HintFound != null)
        //                HintFound(new Hint(this, null, availability.Number, HintTypes.Square));
        //        }
        //    }
        //}

        public override string ToString()
        {
            return string.Format("SquareId: {0} - Number: {1}", SquareId, SudokuNumber.Value);
        }

        #endregion
    }
}