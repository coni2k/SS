using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public interface IHintNew
    {
        SudokuNumber SudokuNumber { get; set; }
        object Source { get; set; }
    }

    public abstract partial class HintNew<T> : IHintNew
    {
        #region - Properties -

        /// <summary>
        /// Hint value
        /// </summary>
        public SudokuNumber Number { get; internal set; }

        public abstract T Source { get; set; }

        #endregion

        #region - Constructors -

        #endregion
    }

    public class SquareHint : HintNew<Square>
    {
        public override Square Source { get; set; }
    }

    public class GroupNumberHint : HintNew<Group.GroupNumber>
    {
        public override Group.GroupNumber Source { get; set; }
    }
}
