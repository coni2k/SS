using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public class Potential
    {
        #region Events

        public delegate void FoundEventHandler(Potential potential);

        #endregion

        #region Properties

        /// <summary>
        /// Potential square
        /// </summary>
        public Square Square { get; internal set; }

        /// <summary>
        /// The group of the potential square
        /// </summary>
        internal Group SquareGroup { get; set; }

        /// <summary>
        /// Potential value of the square
        /// </summary>
        public Number Number { get; internal set; }

        /// <summary>
        /// The type of the potential square
        /// </summary>
        public PotentialTypes PotentialType { get; internal set; }

        #endregion

        #region Constructors

        public Potential() {}

        internal Potential(Square square, Group group, Number number,PotentialTypes potentialType)
        {
            this.Square = square;
            this.SquareGroup = group;
            this.Number = number;
            this.PotentialType = potentialType;
        }

        #endregion
    }
}
