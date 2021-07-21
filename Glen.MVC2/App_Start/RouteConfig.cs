using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Glen.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("fonts/{*pathInfo}");
  //          routes.IgnoreRoute("test/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");

            
            routes.IgnoreRoute("{file}.woff");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{*id}",
                defaults: new { controller = "Installation", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
