namespace SudokuSolver.Engine
{
    public partial class SudokuNumber
    {
        public class SudokuNumberDto
        {
            #region - Properties -

            public int Value { get; private set; }

            public int Count { get; private set; }

            #endregion

            public SudokuNumberDto(SudokuNumber sudokuNumber)
            {
                Value = sudokuNumber.Value;
                Count = sudokuNumber.Count;
            }
        }
    }
}
