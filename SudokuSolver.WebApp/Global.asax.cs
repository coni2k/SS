using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SudokuSolver.WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class SudokuWebApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Routes
            WebApiRouteConfig.RegisterRoutes(GlobalConfiguration.Configuration.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Others
            CacheConfig.RegisterCache();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            FormatterConfig.RegisterFormatters(GlobalConfiguration.Configuration.Formatters);
            ServiceConfig.RegisterServices(GlobalConfiguration.Configuration.Services);

        }
    }
}