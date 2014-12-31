using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.WS;
using Tasks.WS.Controllers;
using System;
using Moq;
using Tasks.Infrastructure.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using FluentAssertions;
using Tasks.Infrastructure;
using System.Web.Http;
using System.Net.Http;

namespace Tasks.WS.Tests.Controllers
{
    [TestClass]
    public class TasksControllerTest
    {
        private Mock<ITasksService> _mockedTasksService;

        private DomainModel.User _creatorUser;
        private DateTime _createdDateTime;
        private List<DomainModel.Task> _tasksCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockedTasksService = new Mock<ITasksService>();

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
        public void GetAllTasks_SimpleCall_ShouldReturnAllTheTasks()
        {
            // Arrange
            _mockedTasksService.Setup(m => m.GetAll()).Returns(_tasksCollection);

            // Action
            var controller = new TasksController(_mockedTasksService.Object);
            var response = controller.GetAllTasks() as OkNegotiatedContentResult<IEnumerable<DomainModel.Task>>;

            // Assert
            response.Should().NotBeNull();
            _mockedTasksService.VerifyAll();
            response.Content.ShouldBeEquivalentTo(_tasksCollection);
        }

        [TestMethod]
        public void GetAllTasks_FilteringByStatus_ShouldReturnTheCorrectTaskCollection()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            UnityConfig.RegisterComponents();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            var server = new HttpServer(config);
            var client = new HttpClient(server);
            var response = client.GetAsync("http://localhost/tasks").Result;

            var message = response.Content.ReadAsStringAsync().Result;

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotAcceptable);
        }

        [TestMethod]
        public void GetTask_ExistingID_ShouldReturnTheCorrectTask()
        {
            // Arrange
            var taskToReturn = _tasksCollection[0];
            _mockedTasksService.Setup(m => m.FindById(taskToReturn.ID)).Returns(taskToReturn);

            // Action
            var controller = new TasksController(_mockedTasksService.Object);
            var response = controller.GetTask(taskToReturn.ID) as OkNegotiatedContentResult<DomainModel.Task>;


            // Assert
            response.Should().NotBeNull("because the response should be of type OkNegotiatedContentResult<DomainModel.Task>");
            _mockedTasksService.VerifyAll();
            response.Content.Should().BeSameAs(taskToReturn, "Because the response should contains the correct task");
        }
        
        [TestMethod]
        public void GetTask_NotExistingID_ShouldReturnNotFoundError()
        {
            // Arrange
            const int notExistingTaskID = 1000;
            _mockedTasksService.Setup(m => m.FindById(notExistingTaskID)).Returns<DomainModel.Task>(null);

            // Action
            var controller = new TasksController(_mockedTasksService.Object);
            var response = controller.GetTask(notExistingTaskID) as NotFoundResult;

            // Assert
            response.Should().NotBeNull("because the response should be of type NotFoundResult");
        }

        [TestMethod]
        public void Add_TaskWithoutATitle_ShouldReturnBadRequestError()
        {
            // Arrange
            var newTaskToAdd = new Models.NewTask {
                Description = "This is a new task", 
                Title = string.Empty, 
                EstimatedHours = 1, 
                RemainingdHours = 2
            };

            _mockedTasksService.Setup(m => m.Add(It.IsAny<DomainModel.Task>())).Throws<InvalidEntityException<DomainModel.Task>>();

            // Action
            var controller = new TasksController(_mockedTasksService.Object);
            var response = controller.Add(newTaskToAdd) as BadRequestResult;

            // Assert
            response.Should().NotBeNull("because the action result should be a BadRequestResult");
            _mockedTasksService.VerifyAll();
        }

        [TestMethod]
        public void Add_ValidTask_ShouldReturnOkAndTheJSonWithTheNewTask()
        {
            // Arrange
            var newTaskToAdd = new Models.NewTask
            {
                Description = "This is a new task",
                Title = "This is the title for the new task",
                EstimatedHours = 1,
                RemainingdHours = 2
            };

            var taskAdded = new DomainModel.Task() {
                Created = _createdDateTime, 
                Creator = _creatorUser, 
                Description = newTaskToAdd.Description, 
                EstimatedHours = newTaskToAdd.EstimatedHours, 
                ID = _tasksCollection.Max(t => t.ID) + 1, 
                Status = DomainModel.TaskStatus.NotStarted, 
                Title = newTaskToAdd.Title    
            };
            
            _mockedTasksService.Setup(m => m.Add(It.IsAny<DomainModel.Task>())).Returns(taskAdded);

            // Action
            var controller = new TasksController(_mockedTasksService.Object);
            var response = controller.Add(newTaskToAdd) as CreatedNegotiatedContentResult<DomainModel.Task>;

            // Assert
            response.Should().NotBeNull("because should be of type OkNegotiatedContentResult<DomainModel.Task>");
            response.Content.Should().BeOfType<DomainModel.Task>();
            response.Content.Should().BeSameAs(taskAdded);
        }

        [TestMethod]
        public void Update_ExistingTask_ShouldReturnOk()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Update_NotExistingTask_ShouldReturnNotFoundError()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ExistingTaskID_ShouldReturnOk()
        {
            // Arrange
            var taskToDelete = _tasksCollection[0];
            _mockedTasksService.Setup(m => m.Delete(taskToDelete.ID));

            // Act
            var controller = new TasksController(_mockedTasksService.Object);
            var response = controller.Delete(taskToDelete.ID) as OkResult;
            
            // Assert
            _mockedTasksService.Verify(m => m.Delete(taskToDelete.ID));
            response.Should().NotBeNull("because should be of type OkResult");
        }

        [TestMethod]
        public void Delete_NotExistingTaskID_ShouldReturnNotFoundError()
        {
            // Arrange
            var taskToDeleteID = 1000;

            _mockedTasksService.Setup(m => m.Delete(taskToDeleteID)).Throws<NotFoundEntityException<DomainModel.Task>>();

            // Act
            var controller = new TasksController(_mockedTasksService.Object);
            var response = controller.Delete(taskToDeleteID) as NotFoundResult;

            // Assert
            response.Should().NotBeNull("because should be of type NotFoundResult");
        }
    }
}
