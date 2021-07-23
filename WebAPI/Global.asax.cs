using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "ServiceRoute",                                 // Route name
                "svc/{controller}/{action}/{id}",              // URL with parameters
                new { id = RouteParameter.Optional },           // Parameter defaults
                new string[] { "WebAPI.ServiceControllers" }    // Controller Namespaces
            );
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
