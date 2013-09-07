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

        IEnumerable<Square> squares = null;
        ICollection<GroupNumber> groupNumbers;

        #endregion

        #region - Events -

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

        internal void UpdateAvailability(SudokuNumber number, Square square, GroupTypes groupType, Square source)
        {
            GroupNumbers
                .Single(groupNumber =>
                    groupNumber.SudokuNumber.Equals(number))
                .Availabilities
                .Single(availability =>
                    availability.Square.Equals(square)).UpdateAvailability(groupType, source);

            // TODO Can this be better, elegant?
            // Only updates the Updated property of the availabilities that uses the main square
            var relatedAvailabilities = GroupNumbers
                .Select(groupNumber => groupNumber.Availabilities.Single(availability => availability.Square.Equals(square)));

            foreach (var availability in relatedAvailabilities)
                availability.Updated = true;
        }

        //internal void RemoveGroupNumberHint()
        //{
        //    foreach (var groupNumber in GroupNumbers)
        //    {
        //        var hintSquare = Sudoku.HintSquares.SingleOrDefault(square => square.AssignType == AssignTypes.GroupNumberHint);

        //        if (hintSquare != null)
        //            hintSquare.Update(Sudoku.ZeroNumber, AssignTypes.Initial);
        //    }
        //}

        internal void SearchGroupNumberHint()
        {
            var lastGroupNumber = GroupNumbers.IfSingleOrDefault(groupNumber => groupNumber.Availabilities.Count(availability => availability.IsAvailable && availability.Square.IsAvailable) == 1);

            if (lastGroupNumber == null)
                return;

            var lastAvailability = lastGroupNumber.Availabilities.Single(availability => availability.IsAvailable && availability.Square.IsAvailable);

            //if (!lastAvailability.Square.IsAvailable)
            //    return;

            var groupType = lastAvailability.GroupNumber.Group.GroupType;

            switch (groupType)
            {
                case GroupTypes.Square:
                    if (lastAvailability.Square.Hints.Any(hint => hint.Type == HintTypes.GroupNumberSquare))
                        return;
                    lastAvailability.Square.Update(lastAvailability.GroupNumber.SudokuNumber, HintTypes.GroupNumberSquare, lastAvailability.GroupNumber);
                    break;
                case GroupTypes.Horizontal:
                    if (lastAvailability.Square.Hints.Any(hint => hint.Type == HintTypes.GroupNumberHorizontal))
                        return;
                    lastAvailability.Square.Update(lastAvailability.GroupNumber.SudokuNumber, HintTypes.GroupNumberHorizontal, lastAvailability.GroupNumber);
                    break;
                case GroupTypes.Vertical:
                    if (lastAvailability.Square.Hints.Any(hint => hint.Type == HintTypes.GroupNumberVertical))
                        return;
                    lastAvailability.Square.Update(lastAvailability.GroupNumber.SudokuNumber, HintTypes.GroupNumberVertical, lastAvailability.GroupNumber);
                    break;
            }

            // Added earlier?
            //if (lastAvailability.Square.IsGroupNumberMethodHint)
            //    return;

            // Add the hint
            //lastAvailability.Square.Update(lastAvailability.GroupNumber.SudokuNumber, AssignTypes.GroupNumberHint);
        }

        public override string ToString()
        {
            return string.Format("Id: {0} - Type: {1}", Id, GroupType.ToString()[0]);
        }

        #endregion
    }
}
