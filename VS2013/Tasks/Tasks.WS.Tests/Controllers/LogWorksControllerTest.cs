using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Tasks.WS.Controllers;
using FluentAssertions;
using Moq;
using Tasks.Infrastructure.LogHours;

namespace Tasks.WS.Tests.Controllers
{
    /// <summary>
    /// To be completed
    /// </summary>
    [TestClass]
    public class LogWorksControllerTest : ControllerTestBase
    {
        private Mock<ILogWorksService> _mockLogWorksService;

        private IEnumerable<DomainModel.WorkSession> _workSessionsCollection;
        private DateTime _createdDateTime;
        private DomainModel.User _creatorUser;


        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            _mockLogWorksService = new Mock<ILogWorksService>();

            _createdDateTime = new DateTime(2014, 12, 31);
            _creatorUser = new DomainModel.User() { 
                UserID = 1,
                Username = "federico.orlandini",
                EmailAddress = "federico.orlandini@gmail.com"
            };
            _workSessionsCollection = new List<DomainModel.WorkSession>()
            {
                new DomainModel.WorkSession() { ID = 1, CommentText = "This is the first logged work session", Created = _createdDateTime, Creator = _creatorUser, Date = new DateTime(2015, 1, 1), Hours = 1, Minutes = 0, Seconds = 0 },
                new DomainModel.WorkSession() { ID = 2, CommentText = "This is the second logged work session", Created = _createdDateTime, Creator = _creatorUser, Date = new DateTime(2015, 1, 2), Hours = 2, Minutes = 0, Seconds = 0 },
                new DomainModel.WorkSession() { ID = 3, CommentText = "This is the third logged work session", Created = _createdDateTime, Creator = _creatorUser, Date = new DateTime(2015, 1, 3), Hours = 3, Minutes = 0, Seconds = 0 }
            };
        }

        [TestMethod]
        public void GetAllByTaskId_ForAnExistingTask_ShouldReturnAOkStatusCode()
        {
            // Arrange
            var taskUnderTestId = 1;

            // Action
            var controller = new LogWorksController();
            var response = controller.GetAllCommentsForTask(taskUnderTestId) as OkNegotiatedContentResult<IEnumerable<DomainModel.TaskComment>>;

            // Assert
            response.Should().NotBeNull();
            _mockLogWorksService.VerifyAll();
            response.Content.ShouldAllBeEquivalentTo(_workSessionsCollection);
        }

        [TestMethod]
        public void Add_ForAnExistingTask_ShouldReturnACreatedStatusCode()
        {
            throw new NotImplementedException();
            // ----- TO BE COMPLETED -----
            //// Arrange
            //var newWorkSessionToAdd = new Models.WorkSession
            //{
            //    Comment = "This is a new work session",
            //    Hours = 1,
            //    Minutes = 0,
            //    Seconds = 0
            //};

            //var workSessionAdded = new DomainModel.WorkSession()
            //{
            //    Created = _createdDateTime,
            //    Creator = _creatorUser,
            //    CommentText = newWorkSessionToAdd.Comment,
            //    Date = _createdDateTime,
            //    Hours = newWorkSessionToAdd.Hours,
            //    Minutes = newWorkSessionToAdd.Minutes,
            //    Seconds = newWorkSessionToAdd.Seconds
            //};

            //_mockLogWorksService.Setup(m => m.Add(It.IsAny<DomainModel.WorkSession>())).Returns(workSessionAdded);

            //var client = ConfigureInMemoryTest(wo: _mockedTasksService.Object);

            //// Action
            //var response = client.PostAsJsonAsync("http://localhost/v1/tasks", newTaskToAdd).Result;

            //// Assert
            //response.StatusCode.Should().Be(HttpStatusCode.Created, "because should return a created code");
            //var content = response.Content.ReadAsAsync<DomainModel.Task>().Result;
            //content.ShouldBeEquivalentTo(taskAdded, "because the action should return the DomainModel.Task added");
        }

        [TestMethod]
        public void Add_ForAnExistingTask_ShouldCallTheServiceToAddTheLogHoursToTheTask()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Add_ForANotExistingTask_ShouldReturnABadRequestStatusCode()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Update_ForAnExistingComment_ShouldReturnOkStatus()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Update_ForAnExistingComment_ShouldCallTheServiceToUpdateTheLogHours()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Update_ForANotExistingComment_ShouldReturnANotFoundStatusCode()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Update_ForANotExistingComment_ShouldNotCallTheServiceToUpdateTheLogHours()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ForAnExistingComment_ShouldCallTheServiceToDeleteTheLogHours()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ForAnExistingComment_ShouldReturnOkStatusCode()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ForANotExistingComment_ShouldNotCallTheServiceToDeleteTheLogHours()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ForANotExistingComment_ShouldReturnTheNotFoundStatusCode()
        {
            throw new NotImplementedException();
        }
    }
}
