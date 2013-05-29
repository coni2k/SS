using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public int SquaresLeft { get; set; }

        public bool Ready { get; set; }

        public bool AutoSolve { get; set; }

        public SudokuDto() { }

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