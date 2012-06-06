using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSP.SudokuSolver.Engine;
using System.ComponentModel.Design.Serialization;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class PotentialContainer
    {
        public int SquareId { get; set; }
        //TODO This is necessary? Cant we find it through SquareId?
        public int SquareGroupId { get; set; }
        public int PotentialValue { get; set;}
        public PotentialTypes PotentialType { get; set; }
    }
}
