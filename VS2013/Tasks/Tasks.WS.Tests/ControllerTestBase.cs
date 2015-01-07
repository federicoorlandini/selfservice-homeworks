using Microsoft.Practices.Unity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Tasks.DataAccess.Caching;
using Tasks.DataAccess.Tasks;
using Tasks.Infrastructure.LogHours;
using Tasks.Infrastructure.Notifications;
using Tasks.Infrastructure.Tasks;
using Tasks.Infrastructure.Validators;
using Tasks.WS.Lib;

namespace Tasks.WS.Tests
{
    /// <summary>
    /// This is a base class for all the controller's unit test. In this class we have the code to
    /// configure in-memory full ASP.NET WebAPI stack tests
    /// </summary>
    public abstract class ControllerTestBase
    {
        private Mock<IExceptionLogger> _mockedExceptionLogger;

        public virtual void TestInitialize()
        {
            _mockedExceptionLogger = new Mock<IExceptionLogger>();
        }

        /// <summary>
        /// This method onfigure the in-memory hosting for the integration tests
        /// </summary>
        /// <param name="tasksService"></param>
        /// <param name="tasksRepository"></param>
        /// <returns></returns>
        protected HttpClient ConfigureInMemoryTest(ITasksService tasksService = null,
            ILogWorksService logWorksService = null,
            ITaskRepository tasksRepository = null,
            IDomainEntityValidator<DomainModel.Task> taskValidator = null,
            INotificationSender notificationSender = null,
            ICacheProvider cache = null)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            // Overwriting the IoC configuration to inject the mocked repository
            var container = new UnityContainer();
            if (tasksRepository != null)
            {
                container.RegisterInstance<ITaskRepository>(tasksRepository);
            }

            if (tasksService == null)
            {
                container.RegisterType<ITasksService, TasksService>();
            }
            else
            {
                container.RegisterInstance<ITasksService>(tasksService);
            }

            if (logWorksService == null)
            {
                container.RegisterType<ILogWorksService, LogWorksService>();
            }
            else
            {
                container.RegisterInstance<ILogWorksService>(logWorksService);
            }

            if (taskValidator != null)
            {
                container.RegisterInstance<IDomainEntityValidator<DomainModel.Task>>(taskValidator);
            }
            else
            {
                container.RegisterType<IDomainEntityValidator<DomainModel.Task>, DomainEntityValidator<DomainModel.Task>>();
            }

            if (notificationSender != null)
            {
                container.RegisterInstance<INotificationSender>(notificationSender);
            }
            else
            {
                container.RegisterType<INotificationSender, EmailNotificationSender>();
            }

            if (cache != null)
            {
                container.RegisterInstance<ICacheProvider>(cache);
            }
            else
            {
                container.RegisterType<ICacheProvider, CacheProvider>();
            }

            config.DependencyResolver = new UnityResolver(container);

            // Replacing the log service with a mocked one
            config.Services.Replace(typeof(IExceptionLogger), _mockedExceptionLogger.Object);

            var server = new HttpServer(config);
            return new HttpClient(server);
        }
    }
}
