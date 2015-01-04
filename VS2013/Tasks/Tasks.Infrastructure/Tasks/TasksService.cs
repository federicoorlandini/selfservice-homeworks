using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DataAccess;
using Tasks.Infrastructure.Validators;

namespace Tasks.Infrastructure.Tasks
{
    public class TasksService : ITasksService
    {
        private IEntityRepository<DomainModel.Task> _taskRepository;
        private IDomainEntityValidator<DomainModel.Task> _validator;

        public TasksService(IEntityRepository<DomainModel.Task> taskRepository, IDomainEntityValidator<DomainModel.Task> validator)
        {
            _taskRepository = taskRepository;
            _validator = validator;
        }

        public IEnumerable<DomainModel.Task> GetAll()
        {
            return _taskRepository.GetAll();
        }

        public IEnumerable<DomainModel.Task> GetAll(DomainModel.TaskStatus status)
        {
            return _taskRepository.GetAll(status);
        }

        public DomainModel.Task FindById(int ID)
        {
            return _taskRepository.FindById(ID);
        }

        public DomainModel.Task Add(DomainModel.Task task)
        {
            // Validate the entity
            if( !_validator.Validate(task) )
            {
                throw new InvalidEntityException<DomainModel.Task>();
            }

            _taskRepository.Add(task);
            return task;
        }

        public DomainModel.Task Update(DomainModel.Task task)
        {
            return task;
        }

        public bool Delete(int ID)
        {
            return false;
        }
    }
}
