﻿namespace SudokuSolver.Engine
{
    /// <summary>
    /// Data transfer object for group number availability
    /// </summary>
    public class GroupNumberAvailabilityDto
    {
        public int GroupId { get; private set; }

        public GroupTypes GroupType { get; private set; }

        public int SudokuNumber { get; private set; }

        public int SquareId { get; private set; }

        public bool IsAvailable { get; private set; }

        public GroupNumberAvailabilityDto(Group.GroupNumber.GroupNumberAvailability availability)
        {
            GroupId = availability.GroupNumber.Group.Id;
            GroupType = availability.GroupNumber.Group.GroupType;
            SudokuNumber = availability.GroupNumber.SudokuNumber.Value;
            SquareId = availability.Square.SquareId;
            IsAvailable = availability.IsAvailable;
        }
    }
}
