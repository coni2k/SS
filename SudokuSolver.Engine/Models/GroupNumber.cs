using System.Collections.Generic;
using System.Globalization;

namespace SudokuSolver.Engine
{
    public class GroupNumber
    {
        ICollection<GroupNumberAvailability> availabilities;

        public Group Group { get; private set; }
        public SudokuNumber SudokuNumber { get; private set; }
        public ICollection<GroupNumberAvailability> Availabilities { get { return availabilities; } }

        internal GroupNumber(Group group, SudokuNumber number)
        {
            Group = group;
            SudokuNumber = number;

            availabilities = new HashSet<GroupNumberAvailability>();
            foreach (var square in group.Squares)
                availabilities.Add(new GroupNumberAvailability(this, square));
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "G: {0} - N: {1}", Group, SudokuNumber);
        }
    }
}
