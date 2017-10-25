using Chaso.Push.Server.Events;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Owin;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.WebApi;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.IO;
using System;
using Microsoft.Owin.Cors;

namespace Chaso.Push.Server
{
    public static class IoC
    {
        private static Container container;
        static IoC()
        {
            // Create the container as usual.
            container = new Container();
        }

        /// <summary>
        /// Setup json config
        /// </summary>
        /// <param name="config"></param>
        public static void SetJsonConfig(this HttpConfiguration config)
        {
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
        /// <summary>
        /// Since this creates an OWIN middleware, it must be invoked before registering
        /// other OWIN middleware like auth.
        /// </summary>
        /// <param name="app">IAppBuilder</param>
        /// <param name="config">HttpConfiguration</param>
        public static void UseOwinContextInjector(this IAppBuilder app, HttpConfiguration config)
        {
            // Create an OWIN middleware to create an execution context scope
            app.Use(async (context, next) =>
            {
                using (var scope = container.BeginExecutionContextScope())
                {
                    await next.Invoke();
                }
            });
            app.UseCors(CorsOptions.AllowAll);
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            // Register your types, for instance using the scoped lifestyle:
            //container.Register<IUserRepository, SqlUserRepository>(Lifestyle.Scoped);
            container.RegisterSingleton(Program.Log);

            //Registe for IHubContext
            container.RegisterSingleton(GlobalHost.ConnectionManager.GetHubContext<EventHub>());

            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(config);

            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        /// <summary>
        /// Get an instance of the given TService
        /// </summary>
        /// <typeparam name="TService">Service must be get instance</typeparam>
        /// <returns></returns>
        public static TService GetInstance<TService>() where TService : class
        {
            return container.GetInstance<TService>();
        }

        /// <summary>
        ///  Generate swagger doc
        /// </summary>
        /// <param name="httpConfig"></param>
        public static void SaveSwaggerJsonDoc(this HttpConfiguration httpConfig)
        {
            var baseAddress = $"{Properties.Settings.Default.ServiceUrl}:{Properties.Settings.Default.ServicePort}/";

            // 1) Apply your WebApi config.
            //WebApiConfig.Register(httpConfig);
            httpConfig.EnsureInitialized();

            // 2) Generate in-memory swagger doc
            var swaggerProvider = new SwaggerGenerator(
                httpConfig.Services.GetApiExplorer(),
                httpConfig.Formatters.JsonFormatter.SerializerSettings,
                new Dictionary<string, Info> { { "v1", new Info { version = "v1", title = "Push Notify" } } },
                new SwaggerGeneratorOptions(
                    // apply your swagger options here ...
                    schemaIdSelector: (type) => type.FriendlyId(true),
                    conflictingActionsResolver: (apiDescriptions) => apiDescriptions.First()
                )
            );
            var swaggerDoc = swaggerProvider.GetSwagger(baseAddress, "v1");

            // 3) Serialize
            var swaggerString = JsonConvert.SerializeObject(
                swaggerDoc,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new[] { new VendorExtensionsConverter() }
                }
            );

            if (!File.Exists(GetPath()))
                File.Delete(GetPath());

            File.WriteAllText(GetPath(), swaggerString);
        }

        private static string GetPath()
        {
            return string.Format(@"{0}\Chaso.Push.Server.json", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
