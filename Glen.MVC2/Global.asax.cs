using System.Globalization;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Glen.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(ConfigurationManager.AppSettings["DefaultThreadCurrentCulture"] ?? "et-EE");
            CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencySymbol = "€";
            CultureInfo.DefaultThreadCurrentCulture.NumberFormat.NumberDecimalSeparator = ".";

        }





    }
}
