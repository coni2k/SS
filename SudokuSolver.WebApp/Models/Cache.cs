using SudokuSolver.Engine;
using SudokuSolver.WebApp.Managers;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.WebApp.Models
{
    /// <summary>
    /// Application cache model
    /// </summary>
    public class Cache
    {
        public IEnumerable<Content> Contents { get; private set; }
        public IEnumerable<Content> InternalContents { get { return Contents.Where(content => !content.IsExternal); } }
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
            SudokuCases = (ICollection<Sudoku>) new SampleCaseManager().GetCases();
        }
    }
}