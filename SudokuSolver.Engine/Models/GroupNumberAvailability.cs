using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public partial class Group
    {
        public partial class GroupNumber
        {
            public class GroupNumberAvailability
            {
                bool availability;

                public GroupNumber GroupNumber { get; private set; }
                public Square Square { get; private set; }
                // public bool IsAvailable { get; set; }
                // public bool IsAvailable { get { return availability; } }
                public bool IsAvailable { get { return availability && Square.IsAvailable; } }
                internal bool Updated { get; set; }

                internal GroupNumberAvailability(GroupNumber groupNumber, Square square)
                {
                    GroupNumber = groupNumber;
                    Square = square;
                    // IsAvailable = true;
                    availability = true;
                }

                internal void UpdateAvailability(bool isAvailable)
                {
                    // IsAvailable = isAvailable;
                    availability = isAvailable;

                    Updated = true;
                }

                public override string ToString()
                {
                    return string.Format("Number: {0:D2} - Square: {1:D2} - IsAvailable: {3}", GroupNumber.SudokuNumber.Value, Square.SquareId, IsAvailable);
                }
            }
        }
    }
}
