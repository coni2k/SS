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
            ICollection<GroupNumberAvailability> availabilities;

            public Group Group { get; private set; }
            public SudokuNumber Number { get; private set; }
            public ICollection<GroupNumberAvailability> Availabilities { get { return availabilities; } }

            public IEnumerable<GroupNumberAvailability> AvailableSquareAvailabilities
            {
                get { return Availabilities.Where(availability => availability.IsAvailable); }
            }

            internal GroupNumber(Group group, SudokuNumber number)
            {
                Group = group;
                Number = number;

                availabilities = new Collection<GroupNumberAvailability>();
                foreach (var square in group.Squares)
                    availabilities.Add(new GroupNumberAvailability(this, square));
            }
        }
    }
}
