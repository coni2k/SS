using System;
using System.ComponentModel.DataAnnotations;

namespace SudokuSolver.Engine
{
    /// <summary>
    /// Data transfer object for Square
    /// </summary>
    public class SquareDto
    {
        // TODO More validation rules?

        [Required]
        public int SquareId { get; set; }

        [Required]
        public int Value { get; set; }

        public AssignType AssignType { get; private set; }

        public SquareDto() { }

        public SquareDto(Square square)
        {
            if (square == null)
                throw new ArgumentNullException("square");

            SquareId = square.SquareId;
            Value = square.SudokuNumber.Value;
            AssignType = square.AssignType;
        }
    }
}