using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public class Availability
    {
        private Square Square { get; set; }
        public Number Number { get; private set; }
        internal Square HorizontalTypeSource { get; set; }
        internal Square VerticalTypeSource { get; set; }
        internal Square SquareTypeSource { get; set; }

        internal Availability(Square square, Number number)
        {
            Square = square;
            Number = number;
        }

        public int SquareId
        {
            get { return Square.Id; }
        }

        public bool IsAvailable
        {
            get { return HorizontalTypeSource == null
                      && VerticalTypeSource == null
                      && SquareTypeSource == null;
            }
        }
    }
}
