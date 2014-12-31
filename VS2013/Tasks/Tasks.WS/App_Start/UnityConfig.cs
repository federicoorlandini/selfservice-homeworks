using Microsoft.Practices.Unity;
using System.Web.Http;
using Tasks.DataAccess;
using Tasks.Infrastructure.Tasks;
using Unity.WebApi;

namespace Tasks.WS
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType(typeof(IEntityRepository<>), typeof(EntityRepository<>));
            container.RegisterType<ITasksService, TasksService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}