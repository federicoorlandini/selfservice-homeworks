using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Tasks.DataAccess.Tasks
{
    public class TasksRepository : IEntityRepository<DomainModel.Task>
    {
        private IDictionary<int, DomainModel.Task> _tasksCollection;

        public TasksRepository()
        {
            // This contructor should initialize a database connection to retrieve the Tasks
            // In this case, we are using a simple in-memory collection
            _tasksCollection = new Dictionary<int, DomainModel.Task>();
        }

        public TasksRepository(IDictionary<int, DomainModel.Task> tasksCollection)
        {
            _tasksCollection = tasksCollection;
        }

        public IEnumerable<DomainModel.Task> GetAll()
        {
            return _tasksCollection.Values;
        }

        public IEnumerable<DomainModel.Task> GetAll(DomainModel.TaskStatus status)
        {
            return _tasksCollection.Where(t => t.Value.Status == status).Select(t => t.Value);
        }

        public void Add(DomainModel.Task entity)
        {
            // This is a very simple way to generate the new ID. This should be provided by the database
            var newID = _tasksCollection.Values.Max(t => t.ID) + 1;
            entity.ID = newID;
            _tasksCollection[newID] = entity;
        }

        public void Delete(DomainModel.Task entity)
        {
            var entityInCollection = FindById(entity.ID);
            if( entityInCollection != null )
            {
                _tasksCollection.Remove(entity.ID);
            }
        }

        public void Update(DomainModel.Task entity)
        {
            var entityInCollection = FindById(entity.ID);
            if( entityInCollection != null )
            {
                Mapper.CreateMap<DomainModel.Task, DomainModel.Task>();
                Mapper.Map<DomainModel.Task, DomainModel.Task>(entity, entityInCollection);
            }
        }

        public DomainModel.Task FindById(int Id)
        {
            DomainModel.Task task;
            var found = _tasksCollection.TryGetValue(Id, out task);
            return (found ? task : null);
        }
    }
}
