using System.Web.Http;

namespace SudokuSolver.WebApp
{
    public class WebApiRouteConfig
    {
        public static string SudokuRouteName = "SudokuRoute";
        public static string DefaultRoute = "DefaultRoute";

        public static void RegisterRoutes(HttpRouteCollection routes)
        {
            // Sudoku route
            routes.MapHttpRoute(
                name: SudokuRouteName,
                routeTemplate: "api/Sudoku/{action}/{sudokuId}/{squareId}",
                defaults: new { controller = "Sudoku", action = RouteParameter.Optional, sudokuId = RouteParameter.Optional, squareId = RouteParameter.Optional }
            );

            // Default route
            routes.MapHttpRoute(
                name: DefaultRoute,
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "Content", id = RouteParameter.Optional }
            );
        }
    }
}