using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SudokuSolver.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Default url to ignore
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Default page for all urls
            routes.MapPageRoute("Default", "{*url}", "~/default.aspx");

        }
    }
}
