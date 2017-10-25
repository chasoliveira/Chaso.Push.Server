using NLog;
using Owin;
using Swashbuckle.Application;
using System.Web.Http;

namespace Chaso.Push.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Program.Log.Info("Start WebApi...");
            // This server will be accessed by clients from other domains, so
            //  we open up CORS. This needs to be before the call to .MapSignalR()!
            //
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            Program.Log.Info("Start SignalR...");
            // Add SignalR to the OWIN pipeline
            //
            app.MapSignalR();

            // Build up the WebAPI middleware
            //
            var config = new HttpConfiguration();

            config.SetJsonConfig();
            config.MapHttpAttributeRoutes();

            app.UseOwinContextInjector(config);

            Program.Log.Info("Start Swagger...");
            config.EnableSwagger(es =>
            {
                
                es.SingleApiVersion("v1", "Chaso Push Server")
                .Description("Push Notification over SignalR, To Notify Clients about an event")
                .Contact(c=> c.Name("chasoliveira"))
                .License(l=>l.Name("MIT"));
                es.IncludeXmlComments(GetXmlCommentsPath());
            }).EnableSwaggerUi();

            config.SaveSwaggerJsonDoc();
            app.UseWebApi(config);
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\Chaso.Push.Server.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
