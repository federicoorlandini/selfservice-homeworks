using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tasks.DataAccess.Tasks;
using FluentAssertions;
using Moq;
using System.Linq;
using Tasks.DataAccess.Caching;
using Tasks.DomainModel;

namespace Tasks.DataAccess.Tests
{
    [TestClass]
    public class TasksRepositoryTest
    {
        private DomainModel.User _creatorUser;
        private IDictionary<int, DomainModel.Task> _tasks;
        private ICollection<DomainModel.Task> _tasksInCache;
        private Mock<ICacheProvider> _mockCache;

        [TestInitialize]
        public void InitializeTest()
        {
            _mockCache = new Mock<ICacheProvider>();

            _creatorUser = new DomainModel.User()
            { 
                UserID = 1,
                Username = "federico.orlandini"
            };

            _tasks = new Dictionary<int, DomainModel.Task>()
            {
                { 1, new DomainModel.Task { 
                    ID = 1,
                    Created = new DateTime(2014, 12, 25), 
                    Creator = _creatorUser, 
                    Description = "This is the first task for our tests - Not Started", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.NotStarted, 
                    Title = "First task" 
                }},
                { 2, new DomainModel.Task { 
                    ID = 2,
                    Created = new DateTime(2014, 12, 27), 
                    Creator = _creatorUser, 
                    Description = "This is the second task for our tests - In Progress", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.InProgress, 
                    Title = "Second task" 
                }},
                { 3, new DomainModel.Task { 
                    ID = 3,
                    Created = new DateTime(2015, 1, 1), 
                    Creator = _creatorUser, 
                    Description = "This is the third task for our tests - Not Started", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.NotStarted, 
                    Title = "Third task" 
                }}
            };

            _tasksInCache = new List<DomainModel.Task>()
            {
                new DomainModel.Task() { 
                    ID = 10, 
                    Created = new DateTime(2014, 12, 31), 
                    Creator = _creatorUser, 
                    Description = "This is the first task in the cache", 
                    EstimatedHours = 11, 
                    Status = DomainModel.TaskStatus.NotStarted, 
                    Title = "Firts task in cache" 
                },
                new DomainModel.Task() { 
                    ID = 11, 
                    Created = new DateTime(2014, 12, 31), 
                    Creator = _creatorUser, 
                    Description = "This is the second task in the cache", 
                    EstimatedHours = 11, 
                    Status = DomainModel.TaskStatus.InProgress, 
                    Title = "Second task in cache" 
                },
                new DomainModel.Task() { 
                    ID = 12, 
                    Created = new DateTime(2014, 12, 31), 
                    Creator = _creatorUser, 
                    Description = "This is the third task in the cache", 
                    EstimatedHours = 11, 
                    Status = DomainModel.TaskStatus.WorkDone, 
                    Title = "Third task in cache" 
                }
            };
        }
        
        [TestMethod]
        public void GetAll_SimpleCallWithOutCacheHit_ShouldReturnAllTheTasks()
        {
            // Arrange
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(TasksRepository.GetAllCacheKeyPatter)).Returns<IEnumerable<DomainModel.Task>>(null);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var allTasks = repository.GetAll();

            // Assert
            allTasks.ShouldAllBeEquivalentTo(_tasks.Values, "because the GetAll() method should return all the tasks in the repository's collection");
        }

        [TestMethod]
        public void GetAll_SimpleCall_ShouldCheckTheCache()
        {
            // Arrange
            var cacheKey = TasksRepository.GetAllCacheKeyPatter;
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns(_tasks.Values);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var allTasks = repository.GetAll();

            // Assert
            _mockCache.VerifyAll();

        }

        [TestMethod]
        public void GetAll_SimpleCallWithCacheHit_ShouldReturnTheCollectionInCache()
        {
            // Arrange
            var cacheKey = TasksRepository.GetAllCacheKeyPatter;
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns(_tasksInCache);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var result = repository.GetAll();

            // Assert
            result.ShouldBeEquivalentTo(_tasksInCache, "because the result should be get from the cache");
        }

        [TestMethod]
        public void GetAll_SimpleCallWithoutCacheHit_ShouldReturnTheCollectionNotInCache()
        {
            // Arrange
            var cacheKey = TasksRepository.GetAllCacheKeyPatter;
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns<IEnumerable<DomainModel.Task>>(null);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var allTasks = repository.GetAll();

            // Assert
            allTasks.ShouldAllBeEquivalentTo(_tasks.Values, "because the GetAll() method should return all the tasks in the repository's collection");
        }

        [TestMethod]
        public void GetAll_SimpleCallWithoutCacheHit_ShouldUpdateTheCacheWithTheRetrievedValues()
        {
            // Arrange
            var cacheKey = TasksRepository.GetAllCacheKeyPatter;
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns<IEnumerable<DomainModel.Task>>(null);
            _mockCache.Setup(m => m.Set<IEnumerable<DomainModel.Task>>(_tasks.Values, cacheKey)).Verifiable();

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var allTasks = repository.GetAll();

            // Assert
            _mockCache.VerifyAll();
        }

        [TestMethod]
        public void GetAll_FilteringByStatusWithoutCacheHit_ShouldReturnTasksWithTheCorrectStatus()
        {
            // Arrange
            var statusUnderTest = DomainModel.TaskStatus.InProgress;
            int[] inProgressTasksID = new int[] { 2 };
            var cacheKey = string.Format(TasksRepository.GetAllWithStatusCacheKeyPatter, (int)statusUnderTest);
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns<IEnumerable<DomainModel.Task>>(null);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var filteredTasks = repository.GetAll(statusUnderTest);

            // Assert
            filteredTasks.Should().HaveCount(inProgressTasksID.Length, "because the result should contains only one task");
            filteredTasks.Should().OnlyContain(t => t.Status == statusUnderTest, "because the result should contains only tasks with the correct status");
        }

        [TestMethod]
        public void GetAll_FilteringByStatus_ShouldCheckTheCache()
        {
            // Arrange
            var taskStatus = DomainModel.TaskStatus.InProgress;
            var filteredTaskInCache = FilterTaskByStatus(_tasksInCache, taskStatus);
            var cacheKey = string.Format(TasksRepository.GetAllWithStatusCacheKeyPatter, (int)taskStatus);
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns(filteredTaskInCache);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var allTasks = repository.GetAll(taskStatus);

            // Assert
            _mockCache.VerifyAll();
        }

        [TestMethod]
        public void GetAll_FilteringByStatusWithCacheHit_ShouldReturnTheCollectionInCache()
        {
            // Arrange
            var taskStatus = DomainModel.TaskStatus.InProgress;
            var filteredTaskInCache = FilterTaskByStatus(_tasksInCache, taskStatus);
            var cacheKey = string.Format(TasksRepository.GetAllWithStatusCacheKeyPatter, (int)taskStatus);
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns(filteredTaskInCache);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var result = repository.GetAll(taskStatus);

            // Assert
            result.ShouldBeEquivalentTo(filteredTaskInCache, "because the task collection that we get should be the one in the cache");
        }

        [TestMethod]
        public void GetAll_FilteringByStatusWithoutCacheHit_ShouldReturnTheTaskCollectionNotInCache()
        {
            // Arrange
            var taskStatus = DomainModel.TaskStatus.InProgress;
            var cacheKey = string.Format(TasksRepository.GetAllWithStatusCacheKeyPatter, (int)taskStatus);
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns<IEnumerable<DomainModel.Task>>(null);
            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var result = repository.GetAll(taskStatus);

            // Assert
            var filteredTask = _tasks.Values.Where(t => t.Status == taskStatus);
            result.ShouldBeEquivalentTo(filteredTask, "because the task collection that we get should be the one in the cache");
        }

        [TestMethod]
        public void GetAll_FilteringByStatusWithoutCacheHit_ShouldUpdateTheCacheWithTheRetrievedValues()
        {
            // Arrange
            var taskStatus = DomainModel.TaskStatus.InProgress;
            var cacheKey = string.Format(TasksRepository.GetAllWithStatusCacheKeyPatter, (int)taskStatus);
            _mockCache.Setup(m => m.Get<IEnumerable<DomainModel.Task>>(cacheKey)).Returns<IEnumerable<DomainModel.Task>>(null);
            var filteredTasks = _tasks.Values.Where(t => t.Status == taskStatus);
            _mockCache.Setup(m => m.Set<IEnumerable<DomainModel.Task>>(filteredTasks, cacheKey)).Verifiable();

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var result = repository.GetAll(taskStatus);

            // Assert
            _mockCache.VerifyAll();
        }

        [TestMethod]
        public void Add_SingleEntity_ShouldAddTheEntityInTheCollection()
        {
            // Arrange
            var taskToAdd = new DomainModel.Task() { 
                Title = "A task to add"
            };
            
            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Add(taskToAdd);

            // Assert
            _tasks.Values.Should().Contain(taskToAdd, "because the new entity should have been added to the repository");
        }

        [TestMethod]
        public void Add_SingleEntity_ShouldUpdateTheCache()
        {
            // Arrange
            var nextValidTaskId = 4;    // See the task collection
            var cacheKey = string.Format(TasksRepository.SingleEntityCacheKeyPatter, nextValidTaskId);
            var taskToAdd = new DomainModel.Task()
            {
                Title = "A task to add"
            };
            _mockCache.Setup(m => m.Set<DomainModel.Task>(taskToAdd, cacheKey)).Verifiable();

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Add(taskToAdd);

            // Assert
            _mockCache.VerifyAll();
        }

        [TestMethod]
        public void Add_SingleEntity_ShouldAssignTheIdToTheNewEntity()
        {
            // Arrange
            var taskToAdd = new DomainModel.Task()
            {
                Title = "A task to add"
            };

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Add(taskToAdd);

            // Assert
            taskToAdd.Should().Match<DomainModel.Task>(t => t.ID != 0, "because the repository should assign a valid ID to the new entity");
        }

        [TestMethod]
        public void Delete_EntityAlreadyInTheRepository_ShouldRemoveTheEntityFromTheRepository()
        {
            // Arrange
            var taskToDelete = new DomainModel.Task() { 
                ID = _tasks.First().Value.ID 
            };

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Delete(taskToDelete.ID);

            // Assert
            _tasks.Values.Should().NotContain(taskToDelete, "because the entity should be removed from the collection");
        }

        [TestMethod]
        public void Delete_EntityAlreadyInTheRepository_ShouldRemoveTheEntityFromTheCache()
        {
            // Arrange
            var taskToDeleteId = _tasks.First().Value.ID;
            var cacheKey = string.Format(TasksRepository.SingleEntityCacheKeyPatter, taskToDeleteId);
            _mockCache.Setup(m => m.Invalidate(cacheKey)).Verifiable();

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Delete(taskToDeleteId);

            // Assert
            _mockCache.VerifyAll();
        }

        [TestMethod]
        public void Delete_EntityNotInTheRepository_ShouldLeaveTheCollectionUntouched()
        {
            // Arrange
            var originalCollection = _tasks.Values.ToArray();
            var taskToDelete = new DomainModel.Task()
            {
                ID = 1000
            };

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Delete(taskToDelete.ID);

            // Assert
            _tasks.Values.ToArray().ShouldAllBeEquivalentTo(originalCollection, "because the collection should not has been changed");
        }

        [TestMethod]
        public void Delete_EntityNotInTheRepository_ShouldNotCallTheCache()
        {
            // Arrange
            var taskToDeleteId = 1000;
            var cacheKey = string.Format(TasksRepository.SingleEntityCacheKeyPatter, taskToDeleteId);
            
            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Delete(taskToDeleteId);

            // Assert
            _mockCache.Verify(m => m.Invalidate(cacheKey), Times.Never());
        }

        [TestMethod]
        public void Update_EntityAlreadyInTheRepository_ShouldUpdateTheEntityInTheCollection()
        {
            // Arrange
            var taskInCollection = _tasks.First().Value;
            var taskToUpdate = new DomainModel.Task() {
                ID = taskInCollection.ID,
                Created = taskInCollection.Created,
                Creator = taskInCollection.Creator, 
                Description = "This is a new description", 
                Title = "This is a new title", 
                Status = DomainModel.TaskStatus.InTest, 
                EstimatedHours = 100
            }; 

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Update(taskToUpdate);

            // Assert
            taskInCollection.Should().NotBeNull("because the entity should be in the collection").And.ShouldBeEquivalentTo(taskToUpdate, opt => opt.ExcludingMissingProperties(), "because the update should update all the properties in the entity");
        }

        [TestMethod]
        public void Update_EntityAlreadyInTheRepository_ShouldUpdateTheCache()
        {
            // Arrange
            var taskToUpdate = _tasks.First().Value;

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Update(taskToUpdate);

            // Assert
            var cacheKey = string.Format(TasksRepository.SingleEntityCacheKeyPatter, taskToUpdate.ID);
            _mockCache.Verify(m => m.Set<DomainModel.Task>(taskToUpdate, cacheKey));
        }

        [TestMethod]
        public void Update_EntityNotInTheRepository_ShouldLeaveTheCollectionUntouched()
        {
            // Arrange
            var originalCollection = _tasks.ToArray();
            var taskToUpdate = new DomainModel.Task()
            {
                ID = 1000,
                Created = DateTime.Now,
                Creator = _creatorUser,
                Description = "This is a description",
                Title = "This is a title",
                Status = DomainModel.TaskStatus.InTest,
                EstimatedHours = 100
            }; 

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Update(taskToUpdate);

            // Assert
            originalCollection.ShouldAllBeEquivalentTo(_tasks, "the tasks collection in the repository should not be changed");
        }

        [TestMethod]
        public void Update_EntityNotInTheRepository_ShouldNotCallTheCache()
        {
            // Arrange
            var taskToUpdate = _tasks.First().Value;
            taskToUpdate.ID = 1000; // Task not in collection

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            repository.Update(taskToUpdate);

            // Assert
            var cacheKey = string.Format(TasksRepository.SingleEntityCacheKeyPatter, taskToUpdate.ID);
            _mockCache.Verify(m => m.Set<DomainModel.Task>(taskToUpdate, cacheKey), Times.Never());
        }

        [TestMethod]
        public void FindById_TheIdIsRelatedToAnEntityAlreadyInTheRepositoryButNotInTheCache_ShouldReturnTheCorrectEntity()
        {
            // Arrange
            var taskToSearch = _tasks.First().Value;
            var cacheKey = string.Format(TasksRepository.SingleEntityCacheKeyPatter, taskToSearch.ID);
            _mockCache.Setup(m => m.Get<DomainModel.Task>(cacheKey)).Returns<DomainModel.Task>(null);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var taskFound = repository.FindById(taskToSearch.ID);

            // Assert
            _mockCache.VerifyAll();
            taskFound.ShouldBeEquivalentTo(taskToSearch, "because the repository should return the correct task");
        }

        [TestMethod]
        public void FindById_ShouldCheckTheCache()
        {
            // Arrange
            var taskToSearch = _tasks.First().Value;
            var cacheKey = string.Format(TasksRepository.SingleEntityCacheKeyPatter, taskToSearch.ID);
            _mockCache.Setup(m => m.Get<DomainModel.Task>(cacheKey)).Returns(taskToSearch);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var taskFound = repository.FindById(taskToSearch.ID);

            // Assert
            _mockCache.VerifyAll();
        }

        [TestMethod]
        public void FindById_TheIdIsRelatedToAnEntityNotInTheCahe_AndNotInTheRepository_ShouldReturnNull()
        {
            // Arrange
            var taskToSearchId = 1000;
            var key = "FindById-1000";
            _mockCache.Setup(m => m.Get<DomainModel.Task>(key)).Returns<DomainModel.Task>(null);

            // Act
            var repository = new TasksRepository(_tasks, _mockCache.Object);
            var taskFound = repository.FindById(taskToSearchId);

            // Assert
            taskFound.Should().BeNull("because the repository should return null if the searched entity is not present");
        }

        #region Helper Method
        private IEnumerable<DomainModel.Task> FilterTaskByStatus(IEnumerable<DomainModel.Task> taskCollection, DomainModel.TaskStatus status)
        {
            return taskCollection.Where(t => t.Status == status);
        }
        #endregion
    }
}
