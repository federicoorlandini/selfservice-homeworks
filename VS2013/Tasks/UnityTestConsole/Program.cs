using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Configuration;
using Tasks.Infrastructure.Tasks;
using Tasks.DomainModel;
using Tasks.DataAccess;

namespace UnityTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var unityContainer = new UnityContainer();
            //unityContainer.LoadConfiguration();
            unityContainer.RegisterType(typeof(IEntityRepository<>), typeof(EntityRepository<>));
            //unityContainer.RegisterType<Tasks.DataAccess.IEntityRepository<Tasks.DomainModel.Task>, Tasks.DataAccess.EntityRepository<Tasks.DomainModel.Task>>();
            unityContainer.RegisterType<ITasksService, TasksService>();
            var obj1 = unityContainer.Resolve<Tasks.DataAccess.IEntityRepository<Tasks.DomainModel.Task>>();
            var obj = unityContainer.Resolve<ITasksService>();
            var collection2 = obj.GetAll();
        }
    }
}
