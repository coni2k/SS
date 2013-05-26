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

        [Required]
        public int Size { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }
    }
}