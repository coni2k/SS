using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public partial class Hint : IHintNew
    {
        #region - Events -

        #endregion

        #region - Properties -

        /// <summary>
        /// Source group number
        /// </summary>
        public Group.GroupNumber GroupNumber { get; private set; }

        /// <summary>
        /// Hint value
        /// </summary>
        public SudokuNumber SudokuNumber { get; internal set; }

        /// <summary>
        /// The type of the hint
        /// </summary>
        public HintTypes Type { get; internal set; }

        #endregion

        #region - Constructors -

        internal Hint(Group.GroupNumber groupNumber, SudokuNumber number)
        {
            GroupNumber = groupNumber;
            SudokuNumber = number;
            Type = HintTypes.GroupNumber;
        }

        internal Hint(SudokuNumber number)
        {
            SudokuNumber = number;
            Type = HintTypes.Square;
        }

        #endregion
    }
}
