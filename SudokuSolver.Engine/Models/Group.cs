using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
                .Single(squareAvailability =>
                    squareAvailability.Square.Equals(square)).UpdateAvailability(isAvailable);

            // CheckGroupNumberAvailabilities();
        }

        internal void CheckGroupNumberAvailabilities()
        {
            if (GroupNumbers.Count(availability => availability.AvailableSquareAvailabilities.Count() == 1) == 1)
            {
                if (HintFound != null)
                {
                    // TODO Naming and how to find the availability ?!
                    var lastGroupNumber = GroupNumbers.Single(groupNumber => groupNumber.AvailableSquareAvailabilities.Count() == 1);
                    var lastAvailability = lastGroupNumber.Availabilities.Single(availability => availability.IsAvailable);

                    Console.WriteLine("P - Id: {0} - Value: {1} - Type: Group", lastAvailability.Square.SquareId, lastGroupNumber.SudokuNumber.Value);

                    if (HintFound != null)
                        HintFound(new Hint(lastAvailability.Square, this, lastGroupNumber.SudokuNumber, HintTypes.Group));
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
