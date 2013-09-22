﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SudokuSolver.Engine
{
    /// <summary>
    /// Group of squares (for square, horizontal, vertical type)
    /// </summary>
    public class Group
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
        public GroupType GroupType { get; private set; }

        public string UniqueId
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}{1}", GroupType.ToString()[0], Id); }
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

                // Availabilities; bit strange place but new GroupNumber will be using Squares property of it's group?
                if (this.GroupType == Engine.GroupType.Square)
                {
                    groupNumbers = new HashSet<GroupNumber>();
                    foreach (var number in Sudoku.NumbersExceptZero)
                        groupNumbers.Add(new GroupNumber(this, number));
                }
            }
        }

        public IEnumerable<GroupNumber> GroupNumbers
        {
            get { return groupNumbers; }
        }

        #endregion

        #region - Constructors -

        internal Group(Sudoku sudoku, int id, GroupType type)
        {
            Sudoku = sudoku;
            Id = id;
            GroupType = type;
        }

        #endregion

        #region - Methods -

        internal void UpdateAvailability(SudokuNumber number, Square square, GroupType groupType, Square source)
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

        //internal void SearchGroupNumberHint()
        //{
        //    var lastGroupNumber = GroupNumbers.IfSingleOrDefault(groupNumber => groupNumber.Availabilities.Count(availability => availability.GetAvailability()) == 1);

        //    if (lastGroupNumber == null)
        //        return;

        //    var lastAvailability = lastGroupNumber.Availabilities.Single(availability => availability.GetAvailability());

        //    var groupType = lastAvailability.GroupNumber.Group.GroupType;

        //    switch (groupType)
        //    {
        //        case DirectionType.Square:
        //            if (lastAvailability.Square.Hints.Any(hint => hint.HintType == HintMethod.NumberMethod))
        //                return;
        //            lastAvailability.Square.AddHint(lastAvailability.GroupNumber.SudokuNumber, HintMethod.NumberMethod, lastAvailability.GroupNumber);
        //            break;
        //        //case GroupType.Horizontal:
        //        //    if (lastAvailability.Square.Hints.Any(hint => hint.HintType == HintType.GroupNumberHorizontal))
        //        //        return;
        //        //    lastAvailability.Square.AddHint(lastAvailability.GroupNumber.SudokuNumber, HintType.GroupNumberHorizontal, lastAvailability.GroupNumber);
        //        //    break;
        //        //case GroupType.Vertical:
        //        //    if (lastAvailability.Square.Hints.Any(hint => hint.HintType == HintType.GroupNumberVertical))
        //        //        return;
        //        //    lastAvailability.Square.AddHint(lastAvailability.GroupNumber.SudokuNumber, HintType.GroupNumberVertical, lastAvailability.GroupNumber);
        //        //    break;
        //    }
        //}

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Id: {0} - Type: {1}", Id, GroupType.ToString()[0]);
        }

        #endregion
    }
}
