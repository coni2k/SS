using System;
using System.Collections;

namespace OSP.SudokuSolver.Engine
{
    /// <summary>
    /// Collection of squares.
    /// </summary>
    public class Squares : CollectionBase
    {
        public int Add(Square value)
        {
            return List.Add(value);
        }

        public Square this[int index]
        {
            get { return List[index] as Square; }
        }

        public void Remove(Square value)
        {
            List.Remove(value);
        }

        public bool Contains(Square value)
        {
            return List.Contains(value);
        }
    }
}
