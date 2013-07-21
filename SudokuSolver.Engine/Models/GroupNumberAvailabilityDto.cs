namespace SudokuSolver.Engine
{
    /// <summary>
    /// Data transfer object for group number availability
    /// </summary>
    public class GroupNumberAvailabilityDto
    {
        public string GroupUniqueId { get; private set; }

        public int Number { get; private set; }

        public int SquareId { get; private set; }

        public bool IsAvailable { get; private set; }

        public GroupNumberAvailabilityDto(Group.GroupNumber.GroupNumberAvailability availability)
        {
            GroupUniqueId = availability.GroupNumber.Group.UniqueId;
            Number = availability.GroupNumber.Number.Value;
            SquareId = availability.Square.SquareId;
            IsAvailable = availability.IsAvailable;
        }
    }
}
