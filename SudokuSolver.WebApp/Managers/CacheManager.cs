using SudokuSolver.WebApp.Models;
using System.Web;

namespace SudokuSolver.WebApp.Managers
{
    public static class CacheManager
    {
        public static Cache Cache
        {
            get { return (Cache)HttpContext.Current.Cache["Cache"]; }
        }
    }
}