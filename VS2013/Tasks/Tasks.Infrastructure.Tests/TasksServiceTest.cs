using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.DataAccess;
using Moq;
using System.Collections.Generic;
using FluentAssertions;
using Tasks.Infrastructure.Tasks;
using Tasks.Infrastructure.Validators;
using System.Linq;
using Tasks.Infrastructure.Notifications;
using Tasks.DataAccess.Tasks;

namespace Tasks.Infrastructure.Tests
{
    [TestClass]
    public class TasksServiceTest
    {
        private Mock<ITaskRepository> _mockRepository;
        private Mock<IDomainEntityValidator<DomainModel.Task>> _mockValidator;
        private Mock<INotificationSender> _mockNotificationSender;

        private DomainModel.User _creatorUser;
        private DateTime _createdDateTime;
        private Dictionary<int, DomainModel.Task> _tasksCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _mockValidator = new Mock<IDomainEntityValidator<DomainModel.Task>>();
            _mockNotificationSender = new Mock<INotificationSender>();

            _creatorUser = new DomainModel.User { UserID = 1, Username = "federico.orlandini" };
            _createdDateTime = new DateTime(2014, 12, 25);


            _tasksCollection = new Dictionary<int, DomainModel.Task>() 
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
        }

        [TestMethod]
        public void GetAll_SimpleCall_ShouldReturnAllTheTaskEntities()
        {
            // Arrange
            _mockRepository.Setup(m => m.GetAll()).Returns(_tasksCollection.Values);

            // Act
            var service =  new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            var result = service.GetAll();

            // Assert
            result.ShouldBeEquivalentTo(_tasksCollection.Values);
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void GetAll_FilteringByStatus_ShouldCallTheCorrectRepositoryMethod()
        {
            // Arrange
            var status = DomainModel.TaskStatus.InProgress;
            var inProgressTasksCollection = new List<DomainModel.Task>() { 
                new DomainModel.Task { 
                    ID = 1,
                    Created = _createdDateTime, 
                    Creator = _creatorUser, 
                    Description = "This is the first task for our tests", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.InProgress, 
                    Title = "First task" 
                },
                new DomainModel.Task { 
                    ID = 2,
                    Created = _createdDateTime, 
                    Creator = _creatorUser, 
                    Description = "This is the second task for our tests", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.InProgress, 
                    Title = "Second task" 
                }
            };
            _mockRepository.Setup(m => m.GetAll(status)).Returns(inProgressTasksCollection);

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            var result = service.GetAll(status);

            // Assert
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void FindById_WithExistingID_ShouldReturnTheCorrectEntity()
        {
            // Arrange
            var taskToFind = _tasksCollection.First().Value;
            _mockRepository.Setup(m => m.FindById(taskToFind.ID)).Returns(taskToFind);

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            var result = service.FindById(taskToFind.ID);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(taskToFind);
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void FindById_WithNotExistingID_ShouldReturnNull()
        {
            // Arrange
            var taskToFindId = 1000;
            _mockRepository.Setup(m => m.FindById(taskToFindId)).Returns<DomainModel.Task>(null);

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            var result = service.FindById(taskToFindId);

            // Assert
            result.Should().BeNull();
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void Add_ValidEntity_ShouldAddTheEntityToTheRepository()
        {
            // Arrange
            var taskToBeAdded = new DomainModel.Task();
            _mockValidator.Setup(m => m.Validate(taskToBeAdded)).Returns(true);
            _mockRepository.Setup(m => m.Add(taskToBeAdded)).Verifiable();

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Add(taskToBeAdded);

            // Assert
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException<DomainModel.Task>))]
        public void Add_NotValidEntity_ShouldThrowAnInvalidEntityException()
        {
            // Arrange
            var taskToBeAdded = new DomainModel.Task();
            _mockValidator.Setup(m => m.Validate(taskToBeAdded)).Returns(false);

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Add(taskToBeAdded);

            // Assert
            Assert.Fail("Should not reah this line of code");
        }

        [TestMethod]
        public void Update_ValidEntity_ShouldUpdateTheEntityInTheRepository()
        {
            // Arrange
            var taskToBeUpdated = _tasksCollection.First().Value;
            _mockValidator.Setup(m => m.Validate(taskToBeUpdated)).Returns(true);
            _mockRepository.Setup(m => m.Update(taskToBeUpdated)).Verifiable();

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Update(taskToBeUpdated);

            // Assert
            _mockValidator.VerifyAll();
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void Update_ValidEntity_ShouldSendTheNotificationsToAllTheWatchers()
        {
            // Arrange
            var taskToBeUpdated = _tasksCollection.First().Value;
            var watcher1 = new DomainModel.User() { UserID = 10, EmailAddress = "federico.orlandini@gmail.com", Username = "federico.orlandini" };
            var watcher2 = new DomainModel.User() { UserID = 11, EmailAddress = "bill.gates@gmail.com", Username = "bill.gates" };
            var watcherUsers = new List<DomainModel.User>() { watcher1, watcher2 };

            _mockValidator.Setup(m => m.Validate(taskToBeUpdated)).Returns(true);
            _mockRepository.Setup(m => m.FindById(taskToBeUpdated.ID)).Returns(taskToBeUpdated);
            _mockRepository.Setup(m => m.GetWatcherUsersForTask(taskToBeUpdated.ID)).Returns(watcherUsers);
            _mockNotificationSender.Setup(m => m.SendUpdateTaskNotification(watcher1, taskToBeUpdated)).Verifiable();
            _mockNotificationSender.Setup(m => m.SendUpdateTaskNotification(watcher2, taskToBeUpdated)).Verifiable();

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Update(taskToBeUpdated);

            // Assert
            _mockNotificationSender.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException<DomainModel.Task>))]
        public void Update_NotValidEntity_ShouldThrowAnInvalidEntityException()
        {
            // Arrange
            var taskToBeUpdated = _tasksCollection.First().Value;
            _mockValidator.Setup(m => m.Validate(taskToBeUpdated)).Returns(false);

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Update(taskToBeUpdated);

            // Assert
            Assert.Fail("Shoud not reach this line of code");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException<DomainModel.Task>))]
        public void Update_NotValidEntity_ShouldNotSendTheNotificationsToTheWatchers()
        {
            // Arrange
            var taskToBeUpdated = _tasksCollection.First().Value;

            _mockValidator.Setup(m => m.Validate(taskToBeUpdated)).Returns(false);
            
            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Update(taskToBeUpdated);

            // Assert
            _mockRepository.Verify(m => m.Update(taskToBeUpdated), Times.Never());
        }

        [TestMethod]
        public void Delete_ExistingEntity_ShouldDeleteTheEntityFromTheRepository()
        {
            // Arrange
            var taskToBeDeleted = _tasksCollection.First().Value;
            _mockRepository.Setup(m => m.FindById(taskToBeDeleted.ID)).Returns(taskToBeDeleted);
            _mockRepository.Setup(m => m.Delete(taskToBeDeleted.ID)).Verifiable();

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Delete(taskToBeDeleted.ID);

            // Assert
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void Delete_ExistingEntity_ShouldSendTheNotificationsToAllTheWatchers()
        {
            // Arrange
            var taskToBeDeleted = _tasksCollection.First().Value;
            var watcher1 = new DomainModel.User() { UserID = 10, EmailAddress = "federico.orlandini@gmail.com", Username = "federico.orlandini" };
            var watcher2 = new DomainModel.User() { UserID = 11, EmailAddress = "bill.gates@gmail.com", Username = "bill.gates" };
            var watcherUsers = new List<DomainModel.User>() { watcher1, watcher2 };

            _mockRepository.Setup(m => m.FindById(taskToBeDeleted.ID)).Returns(taskToBeDeleted);
            _mockRepository.Setup(m => m.GetWatcherUsersForTask(taskToBeDeleted.ID)).Returns(watcherUsers);
            _mockNotificationSender.Setup(m => m.SendDeleteTaskNotification(watcher1, taskToBeDeleted)).Verifiable();
            _mockNotificationSender.Setup(m => m.SendDeleteTaskNotification(watcher2, taskToBeDeleted)).Verifiable();

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Delete(taskToBeDeleted.ID);

            // Assert
            _mockNotificationSender.VerifyAll();
        }

        [TestMethod]
        public void Delete_NotExistingEntity_ShouldNotCallTheDeleteMethodInTheRepository()
        {
            // Arrange
            var unexistingTaskId = 1000;

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Delete(unexistingTaskId);

            // Assert
            _mockRepository.Verify(m => m.Delete(unexistingTaskId), Times.Never());
        }

        [TestMethod]
        public void Delete_NotExistingEntity_ShouldNotSendNotifications()
        {
            // Arrange
            var taskToBeDeleted = _tasksCollection.First().Value;
            var watcher1 = new DomainModel.User() { UserID = 10, EmailAddress = "federico.orlandini@gmail.com", Username = "federico.orlandini" };
            var watcher2 = new DomainModel.User() { UserID = 11, EmailAddress = "bill.gates@gmail.com", Username = "bill.gates" };
            var watcherUsers = new List<DomainModel.User>() { watcher1, watcher2 };

            _mockRepository.Setup(m => m.GetWatcherUsersForTask(taskToBeDeleted.ID)).Returns(watcherUsers);
            _mockNotificationSender.Setup(m => m.SendDeleteTaskNotification(watcher1, taskToBeDeleted)).Verifiable();
            _mockNotificationSender.Setup(m => m.SendDeleteTaskNotification(watcher2, taskToBeDeleted)).Verifiable();

            // Act
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object, _mockNotificationSender.Object);
            service.Delete(taskToBeDeleted.ID);

            // Assert
            _mockNotificationSender.Verify(m => m.SendDeleteTaskNotification(It.IsAny<DomainModel.User>(), It.IsAny<DomainModel.Task>()), Times.Never());
        }
    }
}
