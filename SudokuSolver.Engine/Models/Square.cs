using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace SudokuSolver.Engine
{
    public class Square
    {
        #region - Members -

        AssignType assignType;
        ICollection<Group> groups;
        ICollection<SquareAvailability> availabilities;
        IEnumerable<Square> relatedSquares;
        IEnumerable<Group> relatedGroups;

        #endregion

        #region - Events -

        #endregion

        #region - Properties -

        public List<Hint> Hints { get; private set; }

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

        public AssignType AssignType
        {
            get
            {
                //if (SudokuNumber.IsZero && Hints.Any())
                //    return AssignType.Hint;
                return assignType;
            }
            set
            {
                assignType = value;
            }
        }

        // Groups
        public Group SquareTypeGroup { get; private set; }
        public Group HorizontalTypeGroup { get; private set; }
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
                    groups.Add(HorizontalTypeGroup);
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
            get { return relatedGroups ?? (relatedGroups = RelatedSquares.SelectMany(square => square.Groups.Where(g => g.GroupType == GroupType.Square)).Distinct()); }
        }

        public bool HasHints
        {
            get { return Hints.Any(); }
        }

        public bool IsSquareMethodHint
        {
            get { return AssignType == AssignType.Hint && Hints.Any(hint => hint.HintType != HintType.GroupNumberMethod); }
        }

        public bool IsGroupNumberMethodHint
        {
            get { return AssignType == AssignType.Hint && Hints.Any(hint => hint.HintType == HintType.GroupNumberMethod); }
        }

        #endregion

        #region - Constructors -

        internal Square(int id, Sudoku sudoku, Group squareTypeGroup, Group horizantalTypeGroup, Group verticalTypeGroup)
        {
            Hints = new List<Hint>();

            SquareId = id;
            Sudoku = sudoku;
            SudokuNumber = sudoku.ZeroNumber; // Zero as initial value
            AssignType = AssignType.Initial;

            // Groups
            SquareTypeGroup = squareTypeGroup;
            HorizontalTypeGroup = horizantalTypeGroup;
            VerticalTypeGroup = verticalTypeGroup;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Only changes the assign type
        /// </summary>
        /// <param name="type"></param>
        internal void Update(AssignType type)
        {
            AssignType = type;

            Updated = true;
        }

        internal void Update(SudokuNumber number, AssignType type)
        {
            // Action counter
            // Console.WriteLine("ActionCounter                : {0}", Sudoku.ActionCounter++);
            if (Sudoku.ActionCounter > 50)
                throw new Exception("An infinite loop?");

            // Reset the counters
            Sudoku.SearchSquareHintCounter = 0;
            Sudoku.SearchGroupNumberHintCounter = 0;

            var oldNumber = SudokuNumber;

            // Dump some additional info
            if (Sudoku.DisplaySquareDetails)
                DumpSquareDetails("B - ");
            if (Sudoku.DisplaySquareHints)
                DumpSquareHints("B - ");
            if (Sudoku.DisplaySquareAvailabilities)
                DumpSquareAvailabilities("B - ");
            if (Sudoku.DisplayGroupNumberAvailabilities)
                DumpGroupNumberAvailabilities("B - ");

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
            SearchHints(number);

            // Dump some additional info
            if (Sudoku.DisplaySquareDetails)
                DumpSquareDetails("E - ");
            if (Sudoku.DisplaySquareHints)
                DumpSquareHints("E - ");
            if (Sudoku.DisplaySquareAvailabilities)
                DumpSquareAvailabilities("E - ");
            if (Sudoku.DisplayGroupNumberAvailabilities)
                DumpGroupNumberAvailabilities("E - ");

            //Console.ReadLine();

            // Counters
            //Console.WriteLine("SearchSquareHintCounter      : {0}", Sudoku.SearchSquareHintCounter);
            //Console.WriteLine("SearchGroupNumberHintCounter : {0}", Sudoku.SearchGroupNumberHintCounter);

            if (Sudoku.DisplaySquareDetails || Sudoku.DisplaySquareHints || Sudoku.DisplaySquareAvailabilities || Sudoku.DisplayGroupNumberAvailabilities)
            {
                Console.WriteLine(" - - - - - - - - - - - - - ");
                Console.WriteLine();
            }
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
                        foreach (var squareGroup in square.Groups.Where(g => g.GroupType == GroupType.Square))
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
            foreach (var group in Groups)
            {
                foreach (var square in group.Squares.Where(s => s.Hints.Any() && s.SudokuNumber == number))
                {
                    var hints = square.Hints.ToList();

                    foreach (var hint in hints)
                    {
                        if (hint.HintType == HintType.SquareMethodSquareType)
                        {
                            if (square.Availabilities.Any(a => a.IsSquareTypeAvailable))
                                square.RemoveHint(hint);

                            if (square.Availabilities.Any(a => a.SquareTypeSource.Hints.Any(h => h.HintType== HintType.SquareMethodSquareType)
                                && a.SquareTypeSource.Availabilities.Single(av => av.SudokuNumber == a.SudokuNumber).SquareTypeSource.Equals(square)))
                            {
                                square.RemoveHint(hint);
                            }
                            //var avail = square.Availabilities.Single(a => a.SudokuNumber == SudokuNumber);
                            //if (avail.SquareTypeSource != null &&  avail.SquareTypeSource.Availabilities
                        }

                        if (hint.HintType == HintType.SquareMethodHorizontalType)
                        {
                            if (square.Availabilities.Any(a => a.IsHorizontalTypeAvailable))
                                square.RemoveHint(hint);

                            if (square.Availabilities.Any(a => a.HorizontalTypeSource.Hints.Any(h => h.HintType == HintType.SquareMethodHorizontalType)
                                && a.HorizontalTypeSource.Availabilities.Single(av => av.SudokuNumber == a.SudokuNumber).HorizontalTypeSource.Equals(square)))
                            {
                                square.RemoveHint(hint);
                            }

                        }

                        if (hint.HintType == HintType.SquareMethodVerticalType)
                        {
                            if (square.Availabilities.Any(a => a.IsVerticalTypeAvailable))
                                square.RemoveHint(hint);
                        }
                    }
                }
            }

            //foreach (var square in RelatedSquares.Where(sqr => !sqr.Equals(this) && sqr.IsSquareMethodHint))
            //{
            //    if (square.Availabilities.Single(a => a.SudokuNumber == number).GetAvailability())
            //        square.RemoveHint(HintType.Square);
            //}

            // Group level
            var relatedGroupsSquaresWithHints = RelatedGroups.SelectMany(g => g.Squares.Where(s => s.IsGroupNumberMethodHint && !s.Equals(this))).Distinct();

            foreach (var s in relatedGroupsSquaresWithHints)
            {
                var copyHints = s.Hints.ToList();

                foreach (var hint in copyHints)
                {
                    var src = hint.GroupNumberSource;

                    if (src.Availabilities.Any(avail => avail.GetAvailability(s)))
                        s.RemoveHint(hint);
                }
            }
        }

        void SearchHints(SudokuNumber number)
        {
            if (number.IsZero)
                return;

            // Square level
            foreach (var group in Groups)
            {
                foreach (var square in group.Squares)
                {
                    square.SearchSquareHint(group.GroupType, number);
                }
            }

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
        internal void UpdateAvailability(SudokuNumber number, GroupType type, Square source)
        {
            Availabilities.Single(availability => availability.SudokuNumber.Equals(number)).UpdateAvailability(type, source);
        }

        internal void SearchSquareHint(GroupType type, SudokuNumber number)
        {
            Sudoku.SearchSquareHintCounter++;

            switch (type)
            {
                case GroupType.Square:
                    {
                        var lastAvailability = Availabilities.IfSingleOrDefault(availability => availability.IsSquareTypeAvailable);

                        if (lastAvailability != null)
                        {
                            if (SudokuNumber.IsZero)
                                AddHint(lastAvailability.SudokuNumber, HintType.SquareMethodSquareType, null);

                            return;
                        }

                        break;
                    }
                case GroupType.Horizontal:
                    {
                        var lastAvailability = Availabilities.IfSingleOrDefault(availability => availability.IsHorizontalTypeAvailable);

                        if (lastAvailability != null)
                        {
                            if (SudokuNumber.IsZero)
                                AddHint(lastAvailability.SudokuNumber, HintType.SquareMethodHorizontalType, null);

                            return;
                        }

                        break;
                    }
                case GroupType.Vertical:
                    {
                        var lastAvailability = Availabilities.IfSingleOrDefault(availability => availability.IsVerticalTypeAvailable);

                        if (lastAvailability != null)
                        {
                            if (SudokuNumber.IsZero)
                                AddHint(lastAvailability.SudokuNumber, HintType.SquareMethodVerticalType, null);

                            return;
                        }

                        break;
                    }
            }

            if (!SudokuNumber.IsZero)
            {
                switch (type)
                {
                    case GroupType.Square:
                        {
                            //if (!Availabilities.Any(a => a.IsAvailable)
                            //    && !Availabilities.Single(a => a.SudokuNumber == SudokuNumber).IsSquareTypeAvailable
                            if (!Availabilities.Any(a => a.IsSquareTypeAvailable)                                
                                && !Hints.Any(h => h.HintType == HintType.SquareMethodSquareType))
                                AddHint(SudokuNumber, HintType.SquareMethodSquareType, null);
                            break;
                        }
                    case GroupType.Horizontal:
                        {
                            //if (!Availabilities.Any(a => a.IsAvailable)
                            //    && !Availabilities.Single(a => a.SudokuNumber == SudokuNumber).IsHorizontalTypeAvailable
                            if (!Availabilities.Any(a => a.IsHorizontalTypeAvailable)
                                && !Hints.Any(h => h.HintType == HintType.SquareMethodHorizontalType))
                                AddHint(SudokuNumber, HintType.SquareMethodHorizontalType, null);
                            break;
                        }
                    case GroupType.Vertical:
                        {
                            //if (!Availabilities.Any(a => a.IsAvailable)
                            //    && !Availabilities.Single(a => a.SudokuNumber == SudokuNumber).IsVerticalTypeAvailable
                            if (!Availabilities.Any(a => a.IsVerticalTypeAvailable)
                                && !Hints.Any(h => h.HintType == HintType.SquareMethodVerticalType))
                                AddHint(SudokuNumber, HintType.SquareMethodVerticalType, null);
                            break;
                        }
                }
            }
        }

        internal void AddHint(SudokuNumber number, HintType hintType, GroupNumber groupNumberSource)
        {
            // var currentType = AssignType;

            if (!Hints.Any(hint => hint.HintType == hintType))
                Hints.Add(new Hint(this, hintType, groupNumberSource));

            if (SudokuNumber.IsZero && AssignType == AssignType.Initial && Hints.Any())
                Update(number, AssignType.Hint);
        }

        internal void RemoveHint(Hint hint)
        {
            // var currentType = AssignType;

            if (Hints.Contains(hint))
                Hints.Remove(hint);

            //if (Hints.Any(hint => hint.HintType == hintType))
            //    Hints.RemoveAll(hint => hint.HintType == hintType);

            if (!SudokuNumber.IsZero && AssignType == AssignType.Initial && !Hints.Any())
                Update(Sudoku.ZeroNumber, AssignType.Initial);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Id: {0:D2} - N: {1} - A: {2}", SquareId, SudokuNumber, AssignType.ToString()[0]);
        }

        #region - Dump Methods -

        public void DumpSquareDetails()
        {
            DumpSquareDetails(string.Empty);
        }

        public void DumpSquareDetails(string prefix)
        {
            Console.WriteLine("{0}S: {1}", prefix, this);
            Console.WriteLine();
        }

        public void DumpSquareHints()
        {
            DumpSquareHints(string.Empty);
        }

        public void DumpSquareHints(string prefix)
        {
            foreach (var hint in Hints)
                Console.WriteLine("{0}H: {1}", prefix, hint);
            Console.WriteLine();
        }

        public void DumpSquareAvailabilities()
        {
            DumpSquareAvailabilities(string.Empty);
        }

        public void DumpSquareAvailabilities(string prefix)
        {
            foreach (var availabilitiy in Availabilities)
                Console.WriteLine("{0}A: {1}", prefix, availabilitiy);
            Console.WriteLine();
        }

        public void DumpGroupNumberAvailabilities()
        {
            DumpGroupNumberAvailabilities(string.Empty);
        }

        public void DumpGroupNumberAvailabilities(string prefix)
        {
            DumpGroupNumberAvailabilities(prefix, null, null);
        }

        public void DumpGroupNumberAvailabilities(string prefix, int? squareId, int? numberValue)
        {
            var groups = squareId.HasValue
                ? Groups.Where(g => g.Squares.Any(s => s.SquareId == squareId.Value))
                : Groups;

            foreach (var group in groups.Where(g => g.GroupType == GroupType.Square))
            {
                var groupNumbers = numberValue.HasValue
                    ? group.GroupNumbers.Where(gn => gn.SudokuNumber.Value == numberValue.Value)
                    : group.GroupNumbers;

                foreach (var groupNumber in groupNumbers)
                {
                    var availabilities = squareId.HasValue
                        ? groupNumber.Availabilities.Where(a => a.Square.SquareId == squareId.Value)
                        : groupNumber.Availabilities;

                    foreach (var availabilitiy in availabilities)
                        Console.WriteLine("{0}A: {1}", prefix, availabilitiy);
                }
            }
            Console.WriteLine();
        }


        #endregion

        #endregion
    }
}