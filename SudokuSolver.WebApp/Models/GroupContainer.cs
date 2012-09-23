using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SudokuSolver.Engine;

namespace SudokuSolver.WebApp.Models
{
    public class GroupContainer
    {
        public int GroupId { get; set; }
        public int Type { get; set; }
        public List<SquareContainer> Squares { get; set; }

        public GroupContainer()
        {
            Squares = new List<SquareContainer>();
        }
    }
}
