﻿using System;
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
            internal Square SquareTypeSource { get; private set; }
            internal Square HorizontalTypeSource { get; private set; }
            internal Square VerticalTypeSource { get; private set; }
            internal bool Updated { get; set; }

            internal SquareAvailability(Square square, SudokuNumber number)
            {
                Square = square;
                Number = number;
            }

            internal void SetAvailability(GroupTypes type, Square source)
            {
                switch (type)
                {
                    case GroupTypes.Square: SquareTypeSource = source; break;
                    case GroupTypes.Horizontal: HorizontalTypeSource = source; break;
                    case GroupTypes.Vertical: VerticalTypeSource = source; break;
                }

                Updated = true;
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