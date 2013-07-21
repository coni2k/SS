using System.ComponentModel.DataAnnotations;

namespace SudokuSolver.Engine
{
    /// <summary>
    /// Data transfer object for sudoku
    /// </summary>
    public class SudokuDto
    {
        // TODO More validation rules?

        public int SudokuId { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }

        [Required]
        public int Size { get; set; }

        public int SquaresLeft { get; private set; }
        
        public bool Ready { get; private set; }
        
        public bool AutoSolve { get; private set; }

        public SudokuDto() {}

        public SudokuDto(Sudoku sudoku)
        {
            SudokuId = sudoku.SudokuId;
            Title = sudoku.Title;
            Description = sudoku.Description;
            Size = sudoku.Size;
            SquaresLeft = sudoku.SquaresLeft;
            Ready = sudoku.Ready;
            AutoSolve = sudoku.AutoSolve;
        }
    }
}