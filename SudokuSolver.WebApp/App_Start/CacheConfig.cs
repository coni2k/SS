using SudokuSolver.WebApp.Models;
using System.Web;

namespace SudokuSolver.WebApp
{
    public class CacheConfig
    {
        public static void RegisterCache()
        {
            HttpContext.Current.Cache["Cache"] = new Cache();
        }
    }
}