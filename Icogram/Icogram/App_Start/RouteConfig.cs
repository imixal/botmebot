using System.Web.Mvc;
using System.Web.Routing;

namespace Icogram
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Commands", "commands", new { controller = "Command", action = "List" });
            routes.MapRoute("CCommands", "ccommands", new { controller = "CList", action = "CList" });
            routes.MapRoute("CustomerCommands", "customerCommands", new { controller = "CustomerList", action = "List" });
            routes.MapRoute("Companies", "companies", new { controller = "Company", action = "List" });
            routes.MapRoute("Me", "me", new { controller = "User", action = "MyUserProfile" });
            routes.MapRoute("Users", "users", new { controller = "User", action = "List" });
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