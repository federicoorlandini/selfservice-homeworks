using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tasks.WS;
using Tasks.WS.Controllers;
using System;

namespace Tasks.WS.Tests.Controllers
{
    [TestClass]
    public class TasksControllerTest
    {
        [TestMethod]
        public void GetAllTasks_SimpleCall_ShouldReturnAllTheTasks()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetTask_ExistingID_ShouldReturnTheCorrectTask()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetTask_NotExistingID_ShouldReturnNotFoundError()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void GetTask_RequestFromUnauthorizedUser_ShouldReturnNotAuthorizedError()
        {
            throw new NotImplementedException();
        }
    }
}
