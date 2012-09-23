using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SudokuSolver.Engine;

namespace SudokuSolver.WebApp.Models
{
    public class AvailabilityContainer
    {
        public int SquareId { get; set; }
        public int Number { get; set; }
        public bool IsAvailable { get; set; }
    }
}
