using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SudokuSolver.Engine
{
    public partial class Group
    {
        public partial class GroupNumber
        {
            ICollection<GroupNumberAvailability> availabilities;

            public Group Group { get; private set; }
            public SudokuNumber SudokuNumber { get; private set; }
            public ICollection<GroupNumberAvailability> Availabilities { get { return availabilities; } }

            internal GroupNumber(Group group, SudokuNumber number)
            {
                Group = group;
                SudokuNumber = number;

                availabilities = new Collection<GroupNumberAvailability>();
                foreach (var square in group.Squares)
                    availabilities.Add(new GroupNumberAvailability(this, square));
            }
        }
    }
}
