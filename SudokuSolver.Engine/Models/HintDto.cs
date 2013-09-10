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

        public HintDto(Square hintSquare)
        {
            if (hintSquare == null)
                throw new ArgumentNullException("hintSquare");

            SquareId = hintSquare.SquareId;
            Value = hintSquare.SudokuNumber.Value;
            HintType = hintSquare.AssignType == AssignType.Hint ? HintType.Square : HintType.GroupNumberHorizontal;
        }

        #endregion
    }
}