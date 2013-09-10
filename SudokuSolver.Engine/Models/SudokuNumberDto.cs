using System;
namespace SudokuSolver.Engine.Dtos
{
    public class SudokuNumberDto
    {
        #region - Properties -

        public int Value { get; private set; }

        public int Count { get; private set; }

        #endregion

        public SudokuNumberDto(SudokuNumber sudokuNumber)
        {
            if (sudokuNumber == null)
                throw new ArgumentNullException("sudokuNumber");

            Value = sudokuNumber.Value;
            Count = sudokuNumber.Count;
        }
    }
}
