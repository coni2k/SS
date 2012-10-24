﻿using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public class Hint
    {
        #region Events

        public delegate void FoundEventHandler(Hint hint);

        #endregion

        #region Properties

        /// <summary>
        /// Source square
        /// </summary>
        public Square Square { get; internal set; }

        /// <summary>
        /// The group of the square
        /// </summary>
        internal Group SquareGroup { get; set; }

        /// <summary>
        /// Hint value
        /// </summary>
        public Number Number { get; internal set; }

        /// <summary>
        /// The type of the hint
        /// </summary>
        public HintTypes Type { get; internal set; }

        #endregion

        #region Constructors

        public Hint() {}

        internal Hint(Square square, Group group, Number number,HintTypes type)
        {
            this.Square = square;
            this.SquareGroup = group;
            this.Number = number;
            this.Type = type;
        }

        #endregion
    }
}