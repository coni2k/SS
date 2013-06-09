using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public class HintDto
    {
        #region - Properties -

        public int SquareId { get; set; }

        public int Value { get; set; }

        public HintTypes Type { get; set; }

        #endregion

        #region - Constructors -

        public HintDto() { }

        public HintDto(Hint hint)
        {
            SquareId = hint.Square.SquareId;
            Value = hint.Number.Value;
            Type = hint.Type;
        }

        #endregion
    }
}
