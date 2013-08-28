namespace SudokuSolver.Engine
{
    public class HintDto
    {
        #region - Properties -

        public int SquareId { get; private set; }

        public int Value { get; private set; }

        public HintTypes Type { get; private set; }

        #endregion

        #region - Constructors -

        public HintDto(Square hintSquare)
        {
            SquareId = hintSquare.SquareId;
            Value = hintSquare.SudokuNumber.Value;
            Type = hintSquare.AssignType == AssignTypes.SquareHint ? HintTypes.Square : HintTypes.GroupNumber;
        }

        #endregion
    }
}