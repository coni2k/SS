using SudokuSolver.WebApp.Managers;
using SudokuSolver.WebApp.Models;
using System.Web.Http;

namespace SudokuSolver.WebApp.Controllers
{
    public class BaseController : ApiController
    {
        public Cache Cache { get { return CacheManager.Cache; } }
    }
}
