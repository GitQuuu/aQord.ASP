using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace aQord.ASP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Creating a custom route that force the web app to start with danish culture and still keeps the default route incase this one fails  https://stackoverflow.com/questions/1560796/set-culture-in-an-asp-net-mvc-app?rq=1
            //routes.MapRoute(
            //    name: "da-DKCulture",
            //    url: "{language}-{culture}/{controller}/{action}/{id}",
            //    defaults: new { language = "da", culture = "DK", controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }
    }
}
