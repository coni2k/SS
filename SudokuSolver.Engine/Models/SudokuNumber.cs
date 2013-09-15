using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace SudokuSolver.Engine
{
    public class SudokuNumber
    {
        #region - Properties -

        /// <summary>
        /// Gets the parent sudoku class
        /// </summary>
        private Sudoku Sudoku { get; set; }

        /// <summary>
        /// Value of the number
        /// TODO Try to ensure that every number will be used once (one instance per value)
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// This is a special flag for zero value, which will be treated differently in many cases
        /// </summary>
        internal bool IsZero { get { return this == Sudoku.ZeroNumber; } }

        /// <summary>
        /// Determines whether the square value or it's availabilities were updated since the last UpdateSquare method call.
        /// </summary>
        internal bool Updated { get; set; }

        #endregion

        #region - Constructors -

        internal SudokuNumber(Sudoku sudoku, int value)
        {
            Sudoku = sudoku;
            Value = value;
        }

        #endregion

        #region - Methods -

        public int Count
        {
            get { return Sudoku.Squares.Count(s => s.SudokuNumber.Equals(this)); }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:D2}", Value);
        }

        #endregion
    }
}
