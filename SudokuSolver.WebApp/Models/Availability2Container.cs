using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSP.SudokuSolver.Engine;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class Availability2Container
    {
        public int SquareId { get; set; }
        public int Number { get; set; }
        public bool IsAvailable { get; set; }
    }
}
