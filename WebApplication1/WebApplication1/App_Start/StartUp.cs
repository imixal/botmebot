using System.Web.Http;
using Microsoft.Owin;
using Owin;
using WebApplication1;

[assembly: OwinStartup(typeof(StartUp))]


namespace WebApplication1
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();

            configuration.Routes.MapHttpRoute("WebHook", "{controller}");

            app.UseWebApi(configuration);
        }
    }
}