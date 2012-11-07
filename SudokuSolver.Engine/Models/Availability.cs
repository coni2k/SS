using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public class Availability
    {
        private Square Square { get; set; }
        public SudokuNumber Number { get; private set; }
        internal Square SquareTypeSource { get; set; }
        internal Square HorizontalTypeSource { get; set; }
        internal Square VerticalTypeSource { get; set; }

        internal Availability(Square square, SudokuNumber number)
        {
            Square = square;
            Number = number;
        }

        public int SquareId
        {
            get { return Square.SquareId; }
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
