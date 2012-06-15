using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSP.SudokuSolver.Engine;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class GroupNumberAvailabilityContainer
    {
        public int GroupId { get; set; }
        public int Number { get; set; }
        public int Count { get; set; }
    }
}
