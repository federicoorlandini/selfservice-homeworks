using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.DataAccess;
using Moq;
using System.Collections.Generic;
using FluentAssertions;
using Tasks.Infrastructure.Tasks;
using Tasks.Infrastructure.Validators;
using System.Linq;

namespace Tasks.Infrastructure.Tests
{
    [TestClass]
    public class TasksServiceTest
    {
        private Mock<IEntityRepository<DomainModel.Task>> _mockRepository;
        private Mock<IDomainEntityValidator<DomainModel.Task>> _mockValidator;

        private DomainModel.User _creatorUser;
        private DateTime _createdDateTime;
        private Dictionary<int, DomainModel.Task> _tasksCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IEntityRepository<DomainModel.Task>>();
            _mockValidator = new Mock<IDomainEntityValidator<DomainModel.Task>>();

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
            var service =  new TasksService(_mockRepository.Object, _mockValidator.Object);
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
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object);
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
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object);
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
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object);
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
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object);
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
            var service = new TasksService(_mockRepository.Object, _mockValidator.Object);
            service.Add(taskToBeAdded);

            // Assert
            Assert.Fail("Should not reah this line of code");
        }

        [TestMethod]
        public void Update_ValidEntity_ShouldUpdateTheEntityInTheRepository()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Update_ValidEntity_ShouldSendTheNotificationsToAllTheWatchers()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException<DomainModel.Task>))]
        public void Update_NotValidEntity_ShouldThrowAnInvalidEntityException()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntityException<DomainModel.Task>))]
        public void Update_NotValidEntity_ShouldNotSendTheNotificationsToTheWatchers()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ExistingEntity_ShouldDeleteTheEntityFromTheRepository()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ExistingEntity_ShouldShouldSendTheNotificationsToAllTheWatchers()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_NotExistingEntity_ShouldLeaveUnchangedTheEntitiesInTheRepository()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_NotExistingEntity_ShouldNotSendNotifications()
        {
            throw new NotImplementedException();
        }
    }
}
