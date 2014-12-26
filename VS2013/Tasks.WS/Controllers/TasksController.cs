using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tasks.WS.Models;

namespace Tasks.WS.Controllers
{
    [RoutePrefix("tasks")]
    public class TasksController : Controller
    {
        [HttpGet]
        [Route("all")]
        public JsonResult GetAllTasks()
        {
            // TODO - Add the logic here
            return Json(1);
        }

        [HttpGet]
        [Route("{taskID}")]
        public JsonResult GetTask(int taskID)
        {
            // TODO - Add the logic here
            return Json(1);
        }

        [HttpPost]
        [Route("add")]
        public JsonResult Add(NewTask model)
        {
            // TODO - Add the logic here
            return Json(1);
        }

        [HttpPost]
        [Route("update/{taskID}")]
        public JsonResult Update(int taskID, UpdateTask model)
        {
            // TODO - Add the logic here
            return Json(1);
        }

        [HttpDelete]
        [Route("{taskID}")]
        public JsonResult Delete(int taskID)
        {
            // TODO - Add the logic here
            return Json(1);
        }
    }
}
