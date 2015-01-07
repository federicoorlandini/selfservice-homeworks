using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Tasks.DataAccess.Caching;

namespace Tasks.DataAccess.Tasks
{
    public class TasksRepository : ITaskRepository
    {
        private IDictionary<int, DomainModel.Task> _tasksCollection;
        private ICacheProvider _cache;

        // These are the keys that we will use in the cache. These keys should be builded in another way
        public static readonly string GetAllCacheKeyPatter = "TaskRepository:GetAll";
        public static readonly string GetAllWithStatusCacheKeyPatter = "TaskRepository:GetAll:status:{0}";
        public static readonly string SingleEntityCacheKeyPatter = "TaskRepository:TaskId:{0}";
        public static readonly string GetWatcherUsersForTaskCacheKeyPatter = "TaskRepository:GetWatcherUsersForTask:TaskId:{0}";

        public TasksRepository()
        {
            // This contructor should initialize a database connection to retrieve the Tasks
            // In this case, we are using a simple in-memory collection
            _tasksCollection = new Dictionary<int, DomainModel.Task>();
            _cache = new CacheProvider();
        }

        public TasksRepository(IDictionary<int, DomainModel.Task> tasksCollection, ICacheProvider cache)
        {
            _tasksCollection = tasksCollection;
            _cache = cache;
        }

        /// <summary>
        /// Returns all the tasks
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DomainModel.Task> GetAll()
        {
            // Check the cache
            var cacheKey = GetAllCacheKeyPatter;
            var collectionInCache = _cache.Get<IEnumerable<DomainModel.Task>>(cacheKey);
            if( collectionInCache != null )
            {
                return collectionInCache;
            }

            // Not found in cache
            var collection = _tasksCollection.Values;
            _cache.Set(collection, cacheKey);
            return collection;
        }

        /// <summary>
        /// Returns all the task with a particular status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public IEnumerable<DomainModel.Task> GetAll(DomainModel.TaskStatus status)
        {
            // Check the cache
            var cacheKey = string.Format(GetAllWithStatusCacheKeyPatter, (int)status);
            var collectionInCache = _cache.Get<IEnumerable<DomainModel.Task>>(cacheKey); 
            if( collectionInCache != null )
            {
                return collectionInCache;
            }

            // Not found in cache
            var collection = _tasksCollection.Where(t => t.Value.Status == status).Select(t => t.Value);
            _cache.Set(collection, cacheKey);
            return collection;
        }

        public DomainModel.Task Add(DomainModel.Task entity)
        {
            // This is a very simple way to generate the new ID. This should be provided by the database
            var newID = GetNextID();
            entity.ID = newID;
            entity.Created = DateTime.Now;
            _tasksCollection[newID] = entity;

            // Update the cache
            var cacheKey = string.Format(SingleEntityCacheKeyPatter, entity.ID);
            _cache.Set(entity, cacheKey);

            return entity;
        }

        public void Delete(int entityId)
        {
            var entityInCollection = FindById(entityId);
            if( entityInCollection != null )
            {
                _tasksCollection.Remove(entityId);

                // Remove from the cache
                var cacheKey = string.Format(SingleEntityCacheKeyPatter, entityId);
                _cache.Invalidate(cacheKey);
            }
        }

        public void Update(DomainModel.Task entity)
        {
            var entityInCollection = FindById(entity.ID);
            if( entityInCollection != null )
            {
                Mapper.CreateMap<DomainModel.Task, DomainModel.Task>();
                Mapper.Map<DomainModel.Task, DomainModel.Task>(entity, entityInCollection);

                // Update the cache
                var cacheKey = string.Format(SingleEntityCacheKeyPatter, entity.ID);
                _cache.Set(entity, cacheKey);
            }
        }

        public DomainModel.Task FindById(int entityId)
        {
            // Check the cache
            var cacheKey = string.Format(SingleEntityCacheKeyPatter, entityId);
            var taskInCache = _cache.Get<DomainModel.Task>(cacheKey);
            if( taskInCache != null )
            {
                return taskInCache;
            }

            // Not found in cache
            DomainModel.Task task;
            var found = _tasksCollection.TryGetValue(entityId, out task);
            return (found ? task : null);
        }

        /// <summary>
        /// Returns a collection of users that are the watcher user for the particuar task with the ID passed as parameter
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public IEnumerable<DomainModel.User> GetWatcherUsersForTask(int taskId)
        {
            // This is a dummy implementation
            return new List<DomainModel.User>();
        }

        #region Helper Methods
        private int GetNextID()
        {
            return (_tasksCollection.Any() ? _tasksCollection.Values.Max(t => t.ID) + 1 : 1);
        }
        #endregion
    }
}
