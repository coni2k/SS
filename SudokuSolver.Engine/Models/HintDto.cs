using System;

namespace SudokuSolver.Engine.Dtos
{
    public class HintDto
    {
        #region - Properties -

        public int SquareId { get; private set; }

        public int Value { get; private set; }

        public HintType HintType { get; private set; }

        #endregion

        #region - Constructors -

        public HintDto(Hint hint)
        {
            if (hint == null)
                throw new ArgumentNullException("hint");

            SquareId = hint.Square.SquareId;
            Value = hint.Square.SudokuNumber.Value;
            HintType = hint.HintType;
        }

        #endregion
    }
}