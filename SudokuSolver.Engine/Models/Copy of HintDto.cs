namespace SudokuSolver.Engine
{
    public partial class HintNew
    {
        public class HintNewDto
        {
            #region - Properties -

            public int SquareId { get; private set; }

            public int Value { get; private set; }

            public HintTypes Type { get; private set; }

            #endregion

            #region - Constructors -

            public HintNewDto(Hint hint)
            {
                SquareId = hint.Square.SquareId;
                Value = hint.Number.Value;
                Type = hint.Type;
            }

            #endregion
        }
    }
}