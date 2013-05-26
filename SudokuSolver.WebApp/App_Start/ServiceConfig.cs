using System.Web.Http.Controllers;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;

namespace SudokuSolver.WebApp
{
    public class ServiceConfig
    {
        public static void RegisterServices(ServicesContainer services)
        {
            // Remove InvalidModelValidatorProvider to be able to use "Required" attribute;
            // More info; http://aspnetwebstack.codeplex.com/workitem/270
            services.RemoveAll(typeof(ModelValidatorProvider), v => v is InvalidModelValidatorProvider);
        }
    }
}
