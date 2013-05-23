using System.Web.Http;

namespace SudokuSolver.WebApp
{
    public class WebApiConfig
    {
        public static string RouteNameControllerActionId = "ControllerActionId";

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

            //// Controller
            //config.Routes.MapHttpRoute(
            //    name: RouteNameControllerId,
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { controller = "SudokuList", id = RouteParameter.Optional }
            //);

            //// Controller
            //config.Routes.MapHttpRoute(
            //    name: RouteNameControllerAction,
            //    routeTemplate: "api/{controller}/{action}",
            //    defaults: new { controller = "SudokuList", action = "Reset" }
            //);

            // Controller + Action
            config.Routes.MapHttpRoute(
                name: RouteNameControllerActionId,
                routeTemplate: "api/{controller}/{action}/{id}/{squareId}",
                defaults: new { controller = "SudokuList", action = RouteParameter.Optional, id = RouteParameter.Optional, squareId = RouteParameter.Optional }

            );
        }
    }
}