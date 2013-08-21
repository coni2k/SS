using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    /// <summary>
    /// Group of squares (for square, horizontal, vertical type)
    /// </summary>
    public partial class Group
    {
        #region - Members -

        private IEnumerable<Square> squares = null;
        private ICollection<GroupNumber> groupNumbers;

        #endregion

        #region - Events -

        // internal delegate void GroupSquareEventHandler(Group group, Square square);
        // internal event Hint.FoundEventHandler HintFound;

        #endregion

        #region - Properties -

        /// <summary>
        /// Parent sudoku class
        /// </summary>
        private Sudoku Sudoku { get; set; }

        /// <summary>
        /// Id of the group
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Type of the group
        /// </summary>
        public GroupTypes GroupType { get; private set; }

        public string UniqueId
        {
            get { return string.Format("{0}{1}", GroupType.ToString()[0], Id); }
        }

        /// <summary>
        /// List of squares that group contains
        /// </summary>
        public IEnumerable<Square> Squares
        {
            get
            {
                return squares;
            }
            internal set
            {
                if (squares != null)
                    throw new InvalidOperationException("Squares already initialized");

                squares = value;

                // Availabilities; new GroupNumber will be using Squares property of it's group, that why do this here?
                groupNumbers = new Collection<GroupNumber>();
                foreach (var number in Sudoku.NumbersExceptZero)
                    groupNumbers.Add(new GroupNumber(this, number));
            }
        }

        public IEnumerable<GroupNumber> GroupNumbers
        {
            get { return groupNumbers; }
        }

        #endregion

        #region - Constructors -

        internal Group(Sudoku sudoku, int id, GroupTypes type)
        {
            Sudoku = sudoku;
            Id = id;
            GroupType = type;
        }

        #endregion

        #region - Methods -

        internal void UpdateAvailability(SudokuNumber number, Square square, bool isAvailable)
        {
            GroupNumbers
                .Single(groupNumber =>
                    groupNumber.SudokuNumber.Equals(number))
                .Availabilities
                .Single(availability =>
                    availability.Square.Equals(square)).UpdateAvailability(isAvailable);

            // TODO Can this be better, elegant?
            // Only updates the Updated property of the availabilities that uses the main square
            var relatedAvailabilities = GroupNumbers
                .Select(groupNumber => groupNumber.Availabilities.Single(availability => availability.Square.Equals(square)));

            foreach (var availability in relatedAvailabilities)
                availability.Updated = true;                    
        }

        internal void SearchGroupNumberHint()
        {
            Sudoku.SearchGroupNumberHintCounter++;

            var lastGroupNumber = GroupNumbers.IfSingleOrDefault(groupNumber => groupNumber.Availabilities.Count(availability => availability.IsAvailable) == 1);

            if (lastGroupNumber == null)
                return;

            var lastAvailability = lastGroupNumber.Availabilities.Single(availability => availability.IsAvailable);

            // The square is available?
            if (!lastAvailability.Square.IsAvailable)
                return;

            // Added earlier?
            if (Sudoku.Hints.Any(hint => hint.Square.Equals(lastAvailability.Square) && hint.SquareGroup == lastAvailability.GroupNumber.Group && hint.Type == HintTypes.Group))
                return;

            // Old version
            Sudoku.Hints.Add(new Hint(lastAvailability.Square, lastAvailability.GroupNumber.Group, lastAvailability.GroupNumber.SudokuNumber, HintTypes.Group));

            // New version
            // TODO!

            //if (GroupNumbers.Count(groupNumber => groupNumber.Availabilities.Count(availability => availability.IsAvailable) == 1) == 1)
            //{
            //    if (HintFound != null)
            //    {
            //        var lastAvailability = GroupNumbers.Single(groupNumber => groupNumber
            //            .Availabilities.Count(availability => availability.IsAvailable) == 1)
            //            .Availabilities.Single(availability => availability.IsAvailable);

            //        // TODO lastAvailability . Square . IsAvailable

            //        if (HintFound != null)
            //            HintFound(new Hint(lastAvailability.Square, this, lastAvailability.GroupNumber.SudokuNumber, HintTypes.Group));
            //    }
            //}
        }

        public override string ToString()
        {
            return string.Format("Id: {0} - Type: {1}", Id, GroupType);
        }

        #endregion
    }
}
