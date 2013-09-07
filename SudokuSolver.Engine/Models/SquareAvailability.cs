using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public partial class Square
    {
        public class SquareAvailability
        {
            internal Square Square { get; private set; }
            // TODO Internal ?
            public SudokuNumber Number { get; private set; }
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

            internal SquareAvailability(Square square, SudokuNumber number)
            {
                Square = square;
                Number = number;
            }

            internal void UpdateAvailability(GroupTypes type, Square source)
            {
                switch (type)
                {
                    case GroupTypes.Square: SquareTypeSource = source; break;
                    case GroupTypes.Horizontal: HorizontalTypeSource = source; break;
                    case GroupTypes.Vertical: VerticalTypeSource = source; break;
                }

                Updated = true;
            }

            public override string ToString()
            {
                return string.Format("Square: {0} - Number: {1} - IsAvailable: {2}", Square, Number, IsAvailable);
            }
        }
    }
}
