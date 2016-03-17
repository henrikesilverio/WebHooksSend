using System.Web.Http;

namespace CustomWebHooksSender
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.Indent = true;

            // Load basic support for sending WebHooks
            config.InitializeCustomWebHooks();

            // Load Sql Storage or SQL for persisting subscriptions
            //config.InitializeCustomWebHooksSqlStorage();

            // Load Web API controllers for managing subscriptions
            config.InitializeCustomWebHooksApis();
        }
    }
}