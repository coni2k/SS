using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSP.SudokuSolver.Engine;
using System.Json;
using System.ComponentModel.Design.Serialization;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class PotentialContainer
    {
        public int SquareId { get; set; }
        public int SquareValue { get; set; }
        public int PotentialValue { get; set;}
        public PotentialTypes PotentialType { get; set; }
    }
}
