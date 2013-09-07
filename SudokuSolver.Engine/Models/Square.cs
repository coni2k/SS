using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    public partial class Square
    {
        #region - Members -

        AssignTypes assignType;
        ICollection<Group> groups;
        ICollection<SquareAvailability> availabilities;
        IEnumerable<Square> relatedSquares;
        IEnumerable<Group> relatedGroups;

        #endregion

        #region - Events -

        #endregion

        #region - Properties -

        public List<Hint> Hints { get; set; }
        
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

        public AssignTypes AssignType { get; internal set; }

        ///// <summary>
        ///// Get the assign type of the square
        ///// </summary>
        //public AssignTypes AssignType
        //{
        //    get
        //    {
        //        if (assignType != null)
        //            return assignType; // this can only be solver ?!

        //        // TODO
        //        // if (hints.any() return assigntype.hint;

        //        return Sudoku.Ready
        //            ? AssignTypes.User
        //            : AssignTypes.Initial;
        //    }
        //}

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
            get { return AssignType == AssignTypes.Hint && Hints.Any(hint => hint.Type == HintTypes.Square); }
        }

        public bool IsGroupNumberMethodHint
        {
            get { return AssignType == AssignTypes.Hint && Hints.Any(hint => hint.Type != HintTypes.Square); }
        }

        //public bool IsHint
        //{
        //    get { return IsSquareMethodHint || IsGroupNumberMethodHint; }
        //}

        #endregion

        #region - Constructors -

        internal Square(int id, Sudoku sudoku, Group squareTypeGroup, Group horizantalTypeGroup, Group verticalTypeGroup)
        {
            Hints = new List<Hint>();

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

        //internal void Clear()
        //{
        //    Update(Sudoku.ZeroNumber, AssignTypes.Initial);
        //}

        /// <summary>
        /// Only changes the assign type
        /// </summary>
        /// <param name="type"></param>
        internal void Update(AssignTypes type)
        {
            AssignType = type;

            Updated = true;
        }

        internal void Update(SudokuNumber number, HintTypes hintType, Group.GroupNumber groupNumberSource)
        {
            if (!Hints.Any(hint => hint.Type == hintType))
                Hints.Add(new Hint() { Type = hintType, GroupNumberSource = groupNumberSource });

            if (AssignType != AssignTypes.Hint && Hints.Any())
                Update(number, AssignTypes.Hint);
        }

        internal void Update(HintTypes hintType)
        {
            if (Hints.Any(hint => hint.Type == hintType))
                Hints.RemoveAll(hint => hint.Type == hintType);

            if (AssignType == AssignTypes.Hint && !Hints.Any())
                Update(Sudoku.ZeroNumber, AssignTypes.Initial);
        }
        
        internal void Update(SudokuNumber number, AssignTypes type)
        {
            // Action counter
            // Console.WriteLine("ActionCounter                : {0}", Sudoku.ActionCounter++);
            if (Sudoku.ActionCounter > 50)
                throw new Exception("An infinite loop?");

            // Reset the counters
            Sudoku.SearchSquareHintCounter = 0;
            Sudoku.SearchGroupNumberHintCounter = 0;

            var oldNumber = SudokuNumber;

            Console.WriteLine("B - ID: {0:D2} - OLD: {1:D2} - NEW: {2:D2}", SquareId, oldNumber.Value, number.Value);

            // c. Set the values
            SudokuNumber = number;
            AssignType = type;
            Updated = true;

            // a. Clear availabilities; make the old number available again
            UpdateAvailabilities(null, oldNumber);

            // b. Remove hints
            RemoveHints(oldNumber);

            // d. Update availabilities; make the new number unavailable in the related square / group numbers
            UpdateAvailabilities(this, number);

            // e. Search hints
            SearchHints();

            Console.WriteLine("E - ID: {0:D2} - OLD: {1:D2} - NEW: {2:D2}", SquareId, oldNumber.Value, number.Value);
            Console.WriteLine();

            // Counters
            //Console.WriteLine("SearchSquareHintCounter      : {0}", Sudoku.SearchSquareHintCounter);
            //Console.WriteLine("SearchGroupNumberHintCounter : {0}", Sudoku.SearchGroupNumberHintCounter);
        }

        /// <summary>
        /// Updates the availabilities in related squares and group numbers
        /// </summary>
        void UpdateAvailabilities(Square source, SudokuNumber number)
        {
            // Ignore if it's available
            if (number.IsZero)
                return;

            // Set availabilities of the related squares
            foreach (var group in Groups)
            {
                foreach (var square in group.Squares)
                {
                    if (Sudoku.UseSquareLevelMethod)
                        square.UpdateAvailability(number, group.GroupType, source);

                    if (Sudoku.UseGroupNumberLevelMethod)
                        foreach (var squareGroup in square.Groups)
                            squareGroup.UpdateAvailability(number, square, group.GroupType, source);
                }
            }

            // TODO Can this be better, elegant?
            // Only updates the Updated property of the availabilities of the main square
            foreach (var availability in Availabilities)
                availability.Updated = true;
        }

        void RemoveHints(SudokuNumber number)
        {
            // Ignore if it's available; cannot produce hints
            if (number.IsZero)
                return;

            // Square level
            foreach (var square in RelatedSquares.Where(sqr => !sqr.Equals(this) && sqr.IsSquareMethodHint))
            {
                if (square.Availabilities.Any(avail => avail.IsAvailable))
                    square.Update(HintTypes.Square);
            }

            // Group level
            var relatedGroupsSquaresWithHints = RelatedGroups.SelectMany(g => g.Squares.Where(s => s.IsGroupNumberMethodHint && !s.Equals(this))).Distinct();

            foreach (var s in relatedGroupsSquaresWithHints)
            {
                foreach (var hint in s.Hints)
                {
                    var src = hint.GroupNumberSource;

                    if (src.Availabilities.Any(avail => avail.IsAvailable))
                        s.Update(hint.Type);
                }
            }
        }

        void RemoveHintsOld(SudokuNumber number)
        {
            // Ignore if it's available; cannot produce hints
            if (number.IsZero)
                return;

            //// Square level
            //// Current square must be excluded from this operation, because of it's already going to be updated.
            //// Otherwise it can stuck in an infinite loop by trying to remove its own hint.
            //var relatedSquaresExceptThis = RelatedSquares.Where(square => !square.Equals(this));

            //foreach (var square in relatedSquaresExceptThis)
            //    square.RemoveSquareMethodHint();

            //// Group number level
            //foreach (var group in RelatedGroups)
            //    group.RemoveGroupNumberHint();

            var relatedSquares = RelatedGroups.SelectMany(group => group.Squares.Where(square => !square.Equals(this) && square.AssignType == AssignTypes.Hint)).FirstOrDefault();

            if (relatedSquares != null)
                relatedSquares.Update(Sudoku.ZeroNumber, AssignTypes.Initial);

            //foreach (var square in relatedSquares)
            //{
            //    square.Update(Sudoku.ZeroNumber, AssignTypes.Initial);
            //    break;
            //}
        }

        void SearchHints()
        {
            // Square level
            foreach (var square in RelatedSquares)
                square.SearchSquareHint();

            // Group level
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

        //internal void RemoveSquareMethodHint()
        //{
        //    if (IsSquareMethodHint)
        //        Update(Sudoku.ZeroNumber, AssignTypes.Initial);
        //}

        internal void SearchSquareHint()
        {
            Sudoku.SearchSquareHintCounter++;

            var lastAvailability = Availabilities.IfSingleOrDefault(availability => availability.IsAvailable);

            if (lastAvailability == null)
                return;

            if (!lastAvailability.Square.IsAvailable)
                return;

            // Added earlier?
            if (lastAvailability.Square.IsSquareMethodHint)
                return;

            // Add the hint
            Update(lastAvailability.Number, HintTypes.Square, null);
        }

        public override string ToString()
        {
            return string.Format("SquareId: {0:D2} - Number: {1} - AssignType: {2}", SquareId, SudokuNumber, AssignType);
        }

        #endregion
    }
}