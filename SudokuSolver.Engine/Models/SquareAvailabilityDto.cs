namespace SudokuSolver.Engine
{
    /// <summary>
    /// Data transfer object for square availability
    /// </summary>
    public class SquareAvailabilityDto
    {
        public int SquareId { get; private set; }
        
        public int Value { get; private set; }
        
        public bool IsAvailable { get; private set; }

        public SquareAvailabilityDto(Square.SquareAvailability availability)
        {
            SquareId = availability.Square.SquareId;
            Value = availability.Number.Value;
            IsAvailable = availability.IsAvailable;
        }
    }
}
