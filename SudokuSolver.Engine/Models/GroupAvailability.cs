using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public partial class Group
    {
        public class GroupAvailabilityObsolete
        {
            internal Group Group { get; private set; }
            public SudokuNumber Number { get; private set; }
            internal Square SourceSquare { get; set; }

            internal GroupAvailabilityObsolete(Group group, SudokuNumber number)
            {
                Group = group;
                Number = number;
            }

            public bool IsAvailable
            {
                get { return SourceSquare == null; }
            }

            public override string ToString()
            {
                return string.Format("Number: {0} - IsAvailable: {1}", Number, IsAvailable);
            }
        }

        public class GroupAvailability
        {
            ICollection<GroupSquareAvailability> squareAvailabilities;

            Group Group { get; set; }
            public SudokuNumber Number { get; private set; }
            public ICollection<GroupSquareAvailability> SquareAvailabilities { get { return squareAvailabilities; } }

            public IEnumerable<GroupSquareAvailability> AvailableSquareAvailabilities
            {
                get { return SquareAvailabilities.Where(squareAvailability => squareAvailability.IsAvailable); }
            }

            internal GroupAvailability(Group group, SudokuNumber number)
            {
                Group = group;
                Number = number;

                squareAvailabilities = new Collection<GroupSquareAvailability>();
                foreach (var square in group.Squares)
                    squareAvailabilities.Add(new GroupSquareAvailability(number, square));
            }

            public class GroupSquareAvailability
            {
                SudokuNumber Number { get; set; }
                public Square Square { get; private set; }
                internal bool Availability { get; set; }
                public bool IsAvailable { get { return Square.IsAvailable && Availability; } }

                internal GroupSquareAvailability(SudokuNumber number, Square square)
                {
                    Number = number;
                    Square = square;
                    Availability = true;
                }

                public override string ToString()
                {
                    return string.Format("Number: {0:D2} - Square: {1:D2} - IsAvailable: {3}", Number.Value, Square.SquareId, IsAvailable);
                }
            }
        }
    }
}
