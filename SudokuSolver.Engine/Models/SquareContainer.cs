using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    /// <summary>
    /// Square DTO
    /// </summary>
    public class SquareContainer
    {
        public int SquareId { get; set; }
        public int Value { get; set; }
    }
}