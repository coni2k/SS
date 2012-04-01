using System.Collections.Generic;

namespace OSP.SudokuSolver.Engine
{
    public class Number
    {
        #region Members

        private IList<Square> _AssignedSquares = null;

        #endregion

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

        /// <summary>
        /// Gets the square list which this number has been used
        /// </summary>
        private IEnumerable<Square> AssignedSquares { get { return _AssignedSquares; }}

        #endregion

        #region Constructors

        internal Number(Sudoku sudoku, int value)
        {
            Sudoku = sudoku;
            Value = value;
            _AssignedSquares = new List<Square>(Sudoku.Size);
        }

        #endregion

        #region Methods

        internal void AssignSquare(Square square)
        {
            if (!_AssignedSquares.Contains(square))
                _AssignedSquares.Add(square);
        }

        internal void DeassignSquare(Square square)
        {
            if (_AssignedSquares.Contains(square))
                _AssignedSquares.Remove(square);
        }

        public int Count
        {
            get { return _AssignedSquares.Count; }
        }

        public bool IsAvailable
        {
            get { return !Count.Equals(0); }
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        #endregion

    }
}
