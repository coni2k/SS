using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSP.SudokuSolver.Engine;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class NumberContainer
    {
        public int Value { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
