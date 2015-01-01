using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.DataAccess;
using Moq;
using System.Collections.Generic;
using FluentAssertions;
using Tasks.Infrastructure.Tasks;

namespace Tasks.Infrastructure.Tests
{
    [TestClass]
    public class TasksServiceTest
    {
        private Mock<IEntityRepository<DomainModel.Task>> _mockRepository;

        private DomainModel.User _creatorUser;
        private DateTime _createdDateTime;
        private List<DomainModel.Task> _tasksCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IEntityRepository<DomainModel.Task>>();

            _creatorUser = new DomainModel.User { UserID = 1, Username = "federico.orlandini" };
            _createdDateTime = new DateTime(2014, 12, 25);

            _tasksCollection = new List<DomainModel.Task> { 
                new DomainModel.Task { 
                    ID = 1,
                    Created = _createdDateTime, 
                    Creator = _creatorUser, 
                    Description = "This is the first task for our tests", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.NotStarted, 
                    Title = "First task" 
                },
                new DomainModel.Task { 
                    ID = 2,
                    Created = _createdDateTime, 
                    Creator = _creatorUser, 
                    Description = "This is the second task for our tests", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.NotStarted, 
                    Title = "Second task" 
                },
                new DomainModel.Task { 
                    ID = 3,
                    Created = _createdDateTime, 
                    Creator = _creatorUser, 
                    Description = "This is the third task for our tests", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.NotStarted, 
                    Title = "Third task" 
                }
            };
        }

        [TestMethod]
        public void GetAll_SimpleCall_ShouldReturnAllTheTaskEntities()
        {
            // Arrange
            _mockRepository.Setup(m => m.GetAll()).Returns(_tasksCollection);

            // Act
            var service =  new TasksService(_mockRepository.Object);
            var result = service.GetAll();

            // Assert
            result.ShouldBeEquivalentTo(_tasksCollection);
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
            var service = new TasksService(_mockRepository.Object);
            var result = service.GetAll(status);

            // Assert
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void FindById_WithExistingID_ShouldReturnTheCorrectEntity()
        {
            // Arrange
            var taskToFind = _tasksCollection[0];
            _mockRepository.Setup(m => m.FindById(taskToFind.ID)).Returns(taskToFind);

            // Act
            var service =  new TasksService(_mockRepository.Object);
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
            var service = new TasksService(_mockRepository.Object);
            var result = service.FindById(taskToFindId);

            // Assert
            result.Should().BeNull();
            _mockRepository.VerifyAll();
        }
    }
}
