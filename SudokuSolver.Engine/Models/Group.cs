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

        internal delegate void GroupSquareEventHandler(Group group, Square square);

        internal event Hint.FoundEventHandler HintFound;

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

                // Availabilities
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
            groupNumbers
                .Single(groupNumber =>
                    groupNumber.SudokuNumber.Equals(number))
                .Availabilities
                .Single(availability =>
                    availability.Square.Equals(square)).UpdateAvailability(isAvailable);

            // CheckGroupNumberAvailabilities();
        }

        internal void CheckGroupNumberAvailabilities()
        {
            Sudoku.CheckGroupNumberAvailabilitiesCounter++;

            if (GroupNumbers.Count(groupNumber => groupNumber.Availabilities.Count(availability => availability.IsAvailable) == 1) == 1)
            {
                if (HintFound != null)
                {
                    var lastAvailability = GroupNumbers.Single(groupNumber => groupNumber
                        .Availabilities
                        .Count(availability => availability.IsAvailable) == 1)
                        .Availabilities
                        .Single(availability => availability.IsAvailable);

                    if (HintFound != null)
                        HintFound(new Hint(lastAvailability.Square, this, lastAvailability.GroupNumber.SudokuNumber, HintTypes.Group));
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Id: {0} - Type: {1}", Id, GroupType);
        }

        #endregion
    }
}
