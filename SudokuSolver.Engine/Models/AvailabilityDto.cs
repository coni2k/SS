using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public class AvailabilityDto
    {
        [Required]
        public int SquareId { get; set; }
        
        [Required]
        public int Value { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        public AvailabilityDto() { }

        public AvailabilityDto(Availability availability)
        {
            SquareId = availability.Square.SquareId;
            Value = availability.Number.Value;
            IsAvailable = availability.IsAvailable;
        }
    }
}
