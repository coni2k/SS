using System.ComponentModel.DataAnnotations;

namespace SudokuSolver.Engine
{
    /// <summary>
    /// Data transfer object for Square
    /// </summary>
    public class SquareDto
    {
        // [Required] ?!
        public int SudokuId { get; set; }

        // [Required] ?!
        public int SquareId { get; set; }

        // [Required] ?!
        public int Value { get; set; }
    }
}