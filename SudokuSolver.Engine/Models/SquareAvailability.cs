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
            internal SudokuNumber Number { get; private set; }
            internal Square SquareTypeSource { get; set; }
            internal Square HorizontalTypeSource { get; set; }
            internal Square VerticalTypeSource { get; set; }

            internal SquareAvailability(Square square, SudokuNumber number)
            {
                Square = square;
                Number = number;
            }

            public bool IsAvailable
            {
                get
                {
                    return SquareTypeSource == null
                        && HorizontalTypeSource == null
                        && VerticalTypeSource == null;
                }
            }
        }
    }
}
