using System.Web.Http;

namespace SudokuSolver.WebApp
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}/{squareid}",
                defaults: new { controller = "Sudoku", action = "list", id = RouteParameter.Optional, squareId = RouteParameter.Optional }
            );
        }
    }
}