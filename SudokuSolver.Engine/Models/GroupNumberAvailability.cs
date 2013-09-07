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
                internal Square SquareTypeSource { get; private set; }
                internal Square HorizontalTypeSource { get; private set; }
                internal Square VerticalTypeSource { get; private set; }
                public bool IsAvailable
                {
                    get
                    {
                        return SquareTypeSource == null
                            && HorizontalTypeSource == null
                            && VerticalTypeSource == null;
                            // && Square.IsAvailable;
                    }
                }
                internal bool Updated { get; set; }

                internal GroupNumberAvailability(GroupNumber groupNumber, Square square)
                {
                    GroupNumber = groupNumber;
                    Square = square;
                }

                internal void UpdateAvailability(GroupTypes type, Square source)
                {
                    switch (type)
                    {
                        case GroupTypes.Square: SquareTypeSource = source; break;
                        case GroupTypes.Horizontal: HorizontalTypeSource = source; break;
                        case GroupTypes.Vertical: VerticalTypeSource = source; break;
                    }

                    // TODO Remove
                    //Console.WriteLine("{0} - {1} - {2}", this, type.ToString()[0], source);

                    Updated = true;
                }

                public override string ToString()
                {
                    return string.Format("GroupNumber: {0} - Square: {1} - IsAvailable: {2}", GroupNumber, Square, IsAvailable);
                }
            }
        }
    }
}
