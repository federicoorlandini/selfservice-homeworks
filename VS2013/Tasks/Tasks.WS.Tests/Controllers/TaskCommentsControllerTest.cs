using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.WS.Tests.Controllers
{
    [TestClass]
    public class TaskCommentsControllerTest
    {
        [TestMethod]
        public void GetAllByTaskId_ForAnExistingTask_ShouldReturnAOkStatusCode()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Add_ForAnExistingTask_ShouldReturnACreatedStatusCode()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Add_ForAnExistingTask_ShouldCallTheTaskCommentsServiceToAddTheCommentToTheTask()
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
        public void Update_ForAnExistingComment_ShouldCallTheTaskCommentsServiceToUpdateTheComment()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Update_ForANotExistingComment_ShouldReturnANotFoundStatusCode()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Update_ForANotExistingComment_ShouldNotCallTheTaskCommentsServiceToUpdateTheComment()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ForAnExistingComment_ShouldCallTheTaskCommentsServiceToDeleteTheComment()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ForAnExistingComment_ShouldReturnOkStatusCode()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Delete_ForANotExistingComment_ShouldNotCallTheTaskCommentsServiceToDeleteTheComment()
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
