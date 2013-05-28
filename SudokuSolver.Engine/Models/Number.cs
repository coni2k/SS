using System.Linq;
using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public class SudokuNumber
    {

        #region Properties

        /// <summary>
        /// Value of the number
        /// TODO Try to ensure that every number will be used once (one instance per value)
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets the parent sudoku class
        /// </summary>
        private Sudoku Sudoku { get; set; }

        /// <summary>
        /// This is a special flag for zero value, which will be treated differently in many cases
        /// </summary>
        internal bool IsZero { get { return Value == 0; } }

        #endregion

        #region Constructors

        public SudokuNumber() { }

        internal SudokuNumber(Sudoku sudoku, int value)
        {
            Sudoku = sudoku;
            Value = value;
        }

        #endregion

        #region Methods

        public int Count
        {
            get { return Sudoku.GetSquares().Count(s => s.SudokuNumber.Equals(this)); }
        }

        public override string ToString()
        {
            return string.Format("Value: {0}", Value);
        }

        #endregion

    }
}
