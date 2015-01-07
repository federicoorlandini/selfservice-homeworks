using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DataAccess;
using Tasks.DataAccess.Tasks;
using Tasks.Infrastructure.Notifications;
using Tasks.Infrastructure.Validators;

namespace Tasks.Infrastructure.Tasks
{
    public class TasksService : ITasksService
    {
        private ITaskRepository _taskRepository;
        private IDomainEntityValidator<DomainModel.Task> _validator;
        private INotificationSender _notificationSender;

        public TasksService(ITaskRepository taskRepository, IDomainEntityValidator<DomainModel.Task> validator, INotificationSender notificationSender)
        {
            _taskRepository = taskRepository;
            _validator = validator;
            _notificationSender = notificationSender;
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

            task = _taskRepository.Add(task);
            return task;
        }

        public DomainModel.Task Update(DomainModel.Task task)
        {
            // Validate the entity
            if (!_validator.Validate(task))
            {
                throw new InvalidEntityException<DomainModel.Task>();
            }

            _taskRepository.Update(task);

            // Send the notifications
            var watcherUsers = _taskRepository.GetWatcherUsersForTask(task.ID);
            foreach (var user in watcherUsers)
            {
                _notificationSender.SendUpdateTaskNotification(user, task);
            }

            return task;
        }

        public bool Delete(int taskId)
        {
            var task = FindById(taskId);

            if( task == null )
            {
                return false;
            }

            _taskRepository.Delete(taskId);

            // Send the notifications
            var watcherUsers = _taskRepository.GetWatcherUsersForTask(taskId);
            foreach(var user in watcherUsers)
            {
                _notificationSender.SendDeleteTaskNotification(user, task);
            }

            return true;
        }
    }
}
