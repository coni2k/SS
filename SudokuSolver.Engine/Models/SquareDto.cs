using System.ComponentModel.DataAnnotations;

namespace SudokuSolver.Engine
{
    /// <summary>
    /// Data transfer object for Square
    /// </summary>
    public class SquareDto
    {
        // TODO More validation rules?

        public int SquareId { get; set; }

        [Required]
        public int Value { get; set; }

        public AssignTypes AssignType { get; set; }

        // TODO Availabilities?

        public SquareDto() { }

        public SquareDto(Square square)
        {
            SquareId = square.SquareId;
            Value = square.SudokuNumber.Value;
            AssignType = square.AssignType;
        }
    }
}