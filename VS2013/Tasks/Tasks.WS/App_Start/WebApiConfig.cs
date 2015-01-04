using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Tasks.DataAccess;
using Tasks.Infrastructure.Tasks;
using Tasks.WS.Lib;

namespace Tasks.WS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Unity configuration
            var container = new UnityContainer();
            container.RegisterType(typeof(IEntityRepository<DomainModel.Task>), typeof(DataAccess.Tasks.TasksRepository), new InjectionConstructor());
            container.RegisterType<ITasksService, TasksService>();
            config.DependencyResolver = new UnityResolver(container);

            // enable elmah
            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
