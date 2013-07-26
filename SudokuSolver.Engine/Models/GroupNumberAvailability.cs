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
                public GroupNumber GroupNumber { get; private set; }
                public Square Square { get; private set; }
                internal bool Availability { get; private set; }
                public bool IsAvailable { get { return Square.IsAvailable && Availability; } }
                internal bool Updated { get; set; }

                internal GroupNumberAvailability(GroupNumber groupNumber, Square square)
                {
                    GroupNumber = groupNumber;
                    Square = square;
                    Availability = true;
                }

                internal void SetAvailability(bool isAvailable)
                {
                    Availability = isAvailable;

                    Updated = true;
                }

                public override string ToString()
                {
                    return string.Format("Number: {0:D2} - Square: {1:D2} - IsAvailable: {3}", GroupNumber.Number.Value, Square.SquareId, IsAvailable);
                }
            }
        }
    }
}
