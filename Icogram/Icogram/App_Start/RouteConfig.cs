using System.Web.Mvc;
using System.Web.Routing;

namespace Icogram
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Dashboard", "dashboard", new { controller = "Dashboard", action = "Index" });
            routes.MapRoute("Login", "login", new {controller = "Authentication", action = "Login"});
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}