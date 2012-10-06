using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SudokuSolver.Engine;
using System.Collections.ObjectModel;

namespace SudokuSolver.WebApp
{
    public class CacheManager
    {
        public static ICollection<Sudoku> SudokuList
        {
            get
            {
                if (HttpContext.Current.Cache["SudokuList"] == null)
                    LoadSamples();

                return (ICollection<Sudoku>)HttpContext.Current.Cache["SudokuList"];
            }
        }

        public static void LoadSamples()
        {
            HttpContext.Current.Cache["SudokuList"] = new Cases().GetCases();
        }
    }
}