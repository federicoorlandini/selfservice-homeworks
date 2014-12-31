using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DataAccess;

namespace Tasks.Infrastructure.Tasks
{
    public class TasksService : ITasksService
    {
        private IEntityRepository<DomainModel.Task> _taskRepository;


        public TasksService(IEntityRepository<DomainModel.Task> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<DomainModel.Task> GetAll()
        {
            return _taskRepository.GetAll();
        }

        public DomainModel.Task FindById(int ID)
        {
            return _taskRepository.FindById(ID);
        }

        public DomainModel.Task Add(DomainModel.Task task)
        {
            return null;
        }

        public bool Delete(int ID)
        {
            return false;
        }
    }
}
