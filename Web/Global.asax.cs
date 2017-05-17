using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Infrastructure;

namespace Web
{
    public class MvcApplication : HttpApplication 
    {
        private readonly HashSet<string> areas = new HashSet<string>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas(areas);

            RouteTable.Routes.IgnoreRoute("favicon.ico");
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles, areas);
//            BundleTable.EnableOptimizations = true;

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            Mappings.CreateMappings();
        }
    }
}