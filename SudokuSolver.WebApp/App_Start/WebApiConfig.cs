using System.Web.Http;

namespace SudokuSolver.WebApp
{
    public class WebApiConfig
    {
        //public static string RouteNameController = "Controller";
        public static string RouteNameControllerAction = "ControllerAction";

        public static void Register(HttpConfiguration config)
        {

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}/{squareid}",
            //    defaults: new { controller = "Sudoku", action = "list", id = RouteParameter.Optional, squareId = RouteParameter.Optional }

            //config.Routes.MapHttpRoute(
            //    name: RouteNameController,
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { controller = "SudokuList", id = RouteParameter.Optional }

            //);

            // Controller + Action
            config.Routes.MapHttpRoute(
                name: RouteNameControllerAction,
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "SudokuList", action = "List", id = RouteParameter.Optional }

            );
        }
    }
}