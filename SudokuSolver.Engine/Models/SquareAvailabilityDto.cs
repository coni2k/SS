using System;

namespace SudokuSolver.Engine.Dtos
{
    /// <summary>
    /// Data transfer object for square availability
    /// </summary>
    public class SquareAvailabilityDto
    {
        public int SquareId { get; private set; }
        
        public int Value { get; private set; }
        
        public bool IsAvailable { get; private set; }

        public SquareAvailabilityDto(SquareAvailability availability)
        {
            if (availability == null)
                throw new ArgumentNullException("availability");

            SquareId = availability.Square.SquareId;
            Value = availability.SudokuNumber.Value;
            //IsAvailable = availability.GetAvailability();
            IsAvailable = availability.IsAvailable;
        }
    }
}
