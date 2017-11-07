using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Icogram.Telegram.Bot.Bot;

namespace Icogram
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();


        protected void Application_Start()
        {
            _logger.Info("Application Start");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IcogramBot.SetWebhookAsync().Wait();
        }

        protected void Application_Error()
        {
            var lastException = Server.GetLastError();
            _logger.Fatal(lastException);
        }
    }
}
