using SudokuSolver.Engine;
using SudokuSolver.WebApp.Managers;
using System.Collections.Generic;

namespace SudokuSolver.WebApp.Models
{
    /// <summary>
    /// Application cache model
    /// </summary>
    public class Cache
    {
        public IEnumerable<Content> Contents { get; private set; }
        public ICollection<Sudoku> SudokuCases { get; private set; }

        public Cache()
        {
            Init();
        }

        void Init()
        {
            InitContents();
            InitSudokuCases();
        }

        public void InitContents()
        {
            Contents = new ContentManager().GetContents();
        }

        public void InitSudokuCases()
        {
            SudokuCases = (ICollection<Sudoku>) new CaseManager().GetCases();
        }
    }
}