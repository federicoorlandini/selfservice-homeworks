using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Tasks.Infrastructure;
using Tasks.Infrastructure.Tasks;
using Tasks.WS.Models;
using AutoMapper;

namespace Tasks.WS.Controllers
{
    [RoutePrefix("tasks")]
    [Authorize]
    public class TasksController : ApiController
    {
        private ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;

            // Automapper configuration
            AutoMapper.Mapper.CreateMap<Models.NewTask, DomainModel.Task>();

        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllTasks()
        {
            var tasksCollection = _tasksService.GetAll();
            return Ok(tasksCollection);
        }

        [HttpGet]
        [Route("{taskID}")]
        public IHttpActionResult GetTask(int taskID)
        {
            var task = _tasksService.FindById(taskID);
            if( task == null )
            {
                return NotFound();
            }
            else
            {
                return Ok(task);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Add(NewTask model)
        {
            try
            {
                var newTask = Mapper.Map<Models.NewTask, DomainModel.Task>(model);
                newTask = _tasksService.Add(newTask);
                return Ok(newTask);
            }
            catch(InvalidEntityException<DomainModel.Task>)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{taskID}")]
        public IHttpActionResult Update([FromUri] int taskID, [FromBody] UpdateTask model)
        {
            // TODO - Add the logic here
            return Ok();
        }

        [HttpDelete]
        [Route("{taskID}")]
        public IHttpActionResult Delete([FromUri] int taskID)
        {
            try
            {
                _tasksService.Delete(taskID);
                return Ok();
            }
            catch(NotFoundEntityException<DomainModel.Task>)
            {
                return NotFound();
            }
        }
    }
}
