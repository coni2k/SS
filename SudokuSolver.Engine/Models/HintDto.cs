using System;

namespace SudokuSolver.Engine
{
    public class HintDto
    {
        #region - Properties -

        public int SquareId { get; private set; }

        public int Value { get; private set; }

        public HintType Type { get; private set; }

        #endregion

        #region - Constructors -

        public HintDto(Square hintSquare)
        {
            if (hintSquare == null)
                throw new ArgumentNullException("hintSquare");

            SquareId = hintSquare.SquareId;
            Value = hintSquare.SudokuNumber.Value;
            Type = hintSquare.AssignType == AssignType.Hint ? HintType.Square : HintType.GroupNumberHorizontal;
        }

        #endregion
    }
}