using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public interface IHintNew //<T> where T : class
    {
        SudokuNumber SudokuNumber { get; }
        HintTypes Type { get; }
        // T Source { get; set; }
    }


    public interface IHintNew2 //<T> where T : class
    {
        SudokuNumber SudokuNumber { get; }
        // T Source { get; set; }
    }

    public abstract partial class HintNew<T> : IHintNew2
    {
        #region - Properties -

        /// <summary>
        /// Hint value
        /// </summary>
        public SudokuNumber SudokuNumber { get; internal set; }

        public T Source { get; internal set; }

        #endregion

        #region - Constructors -

        #endregion
    }

    public class SquareHint : HintNew<Square> { }

    public class GroupNumberHint : HintNew<Group.GroupNumber> { }    
}
