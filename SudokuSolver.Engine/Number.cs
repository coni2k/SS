using System.Linq;
using System.Collections.Generic;

namespace OSP.SudokuSolver.Engine
{
    public class Number
    {

        #region Properties

        /// <summary>
        /// Value of the number
        /// TODO Try to ensure that every number will be used once (one instance per value)
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets the parent sudoku class
        /// </summary>
        private Sudoku Sudoku { get; set; }

        /// <summary>
        /// This is a special flag for zero value, which will be treated differently in many cases
        /// </summary>
        public bool IsZero { get { return Value.Equals(0); } }

        #endregion

        #region Constructors

        internal Number(Sudoku sudoku, int value)
        {
            Sudoku = sudoku;
            Value = value;
        }

        #endregion

        #region Methods

        public int GetCount()
        {
            return Sudoku.Squares.Count(s => s.Number.Equals(this));
        }

        public override string ToString()
        {
            return string.Format("Value: {0}", Value.ToString());
        }

        #endregion

    }
}
