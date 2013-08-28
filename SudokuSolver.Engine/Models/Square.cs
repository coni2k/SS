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
        IEnumerable<Square> relatedSquares;
        IEnumerable<Group> relatedGroups;

        #endregion

        #region - Events -

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

        // Groups
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
            get
            {
                if (availabilities == null)
                {
                    // Available numbers; assign all numbers, except zero
                    availabilities = new Collection<SquareAvailability>();
                    foreach (var sudokuNumber in Sudoku.NumbersExceptZero)
                        availabilities.Add(new SquareAvailability(this, sudokuNumber));                
                }

                return availabilities;            
            }
        }

        /// <summary>
        /// Determines whether the square value or it's availabilities were updated since the last UpdateSquare method call.
        /// </summary>
        internal bool Updated { get; set; }

        /// <summary>
        /// Gets the square list within the same groups
        /// </summary>
        public IEnumerable<Square> RelatedSquares
        {
            get { return relatedSquares ?? (relatedSquares = Groups.SelectMany(group => group.Squares).Distinct()); }
        }

        /// <summary>
        /// Gets groups of the related squares
        /// </summary>
        public IEnumerable<Group> RelatedGroups
        {
            get { return relatedGroups ?? (relatedGroups = RelatedSquares.SelectMany(square => square.Groups).Distinct()); }
        }

        public bool IsSquareMethodHint
        {
            get { return AssignType == AssignTypes.SquareHint; }
        }

        public bool IsGroupNumberMethodHint
        {
            get { return AssignType == AssignTypes.GroupNumberHint; }
        }

        public bool IsHint
        {
            get { return IsSquareMethodHint || IsGroupNumberMethodHint; }
        }

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
        }

        #endregion

        #region - Methods -

        internal void Update(AssignTypes type)
        {
            AssignType = type;

            Updated = true;
        }

        internal void Update(SudokuNumber number, AssignTypes type)
        {
            // a. Clear availabilities; make the old number available again
            UpdateAvailabilities(null);

            // b. Remove hints
            if (!SudokuNumber.IsZero && !IsHint)
                RemoveHints();

            // c. Set the values
            SudokuNumber = number;
            AssignType = type;
            Updated = true;

            // d. Update availabilities; make the new number unavailable in the related square / group numbers
            UpdateAvailabilities(this);

            // e. Search hints
            SearchHints();
        }

        /// <summary>
        /// Updates the availabilities in related squares and group numbers
        /// </summary>
        void UpdateAvailabilities(Square source)
        {
            // Ignore zero number, it's not used in availability lists (always available)
            if (SudokuNumber.IsZero)
                return;

            // Set availabilities of the related squares
            foreach (var group in Groups)
            {
                foreach (var square in group.Squares)
                {
                    if (Sudoku.UseSquareLevelMethod)
                        square.UpdateAvailability(SudokuNumber, group.GroupType, source);

                    if (Sudoku.UseGroupNumberLevelMethod)
                        foreach (var squareGroup in square.Groups)
                            squareGroup.UpdateAvailability(SudokuNumber, square, group.GroupType, source);
                }
            }
        }

        void RemoveHints()
        {
            // Square level
            if (Sudoku.UseSquareLevelMethod)
                foreach (var square in RelatedSquares.Where(sqr => !sqr.Equals(this)))
                    square.RemoveSquareMethodHint();

            // Group number level
            if (Sudoku.UseGroupNumberLevelMethod)
                foreach (var group in RelatedGroups)
                    group.RemoveGroupNumberHint();        
        }

        void SearchHints()
        {
            // Square level
            if (Sudoku.UseSquareLevelMethod)
                foreach (var square in RelatedSquares)
                    square.SearchSquareHint();

            // Group level
            if (Sudoku.UseGroupNumberLevelMethod)
                foreach (var group in RelatedGroups)
                    group.SearchGroupNumberHint();        
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

        internal void RemoveSquareMethodHint()
        {
            if (IsSquareMethodHint)
                Update(Sudoku.ZeroNumber, AssignTypes.Initial);
        }

        internal void SearchSquareHint()
        {
            Sudoku.SearchSquareHintCounter++;

            var lastAvailability = Availabilities.IfSingleOrDefault(availability => availability.IsAvailable);

            if (lastAvailability == null)
                return;

            // Added earlier?
            if (lastAvailability.Square.IsSquareMethodHint)
                return;

            // Add the hint
            Update(lastAvailability.Number, AssignTypes.SquareHint);
        }

        public override string ToString()
        {
            return string.Format("SquareId: {0:D2} - Number: {1}", SquareId, SudokuNumber);
        }

        #endregion
    }
}