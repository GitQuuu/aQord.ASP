using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace aQord.ASP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // Configuring App-Service culture into Danish when hosting on azure, and for locally set it in web.config - https://stackoverflow.com/questions/28430862/configuring-azure-web-and-sql-uk-culture?fbclid=IwAR3N7aE-bp4mpWIMcwWNAhqJVncdH7rz8PdORiXlrii3JI5z64EOeqmad2Y
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("da-DK");
        }
    }
}
