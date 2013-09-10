using System;

namespace SudokuSolver.Engine.Dtos
{
    /// <summary>
    /// Data transfer object for group number availability
    /// </summary>
    public class GroupNumberAvailabilityDto
    {
        public int GroupId { get; private set; }

        public GroupType GroupType { get; private set; }

        public int SudokuNumber { get; private set; }

        public int SquareId { get; private set; }

        public bool IsAvailable { get; private set; }

        public GroupNumberAvailabilityDto(GroupNumberAvailability availability)
        {
            if (availability == null)
                throw new ArgumentNullException("availability");

            GroupId = availability.GroupNumber.Group.Id;
            GroupType = availability.GroupNumber.Group.GroupType;
            SudokuNumber = availability.GroupNumber.SudokuNumber.Value;
            SquareId = availability.Square.SquareId;
            IsAvailable = availability.IsAvailable;
        }
    }
}
