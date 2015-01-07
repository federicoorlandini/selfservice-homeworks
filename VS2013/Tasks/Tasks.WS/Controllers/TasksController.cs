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
    [RoutePrefix("v1/tasks")]
    public class TasksController : ApiController
    {
        private ITasksService _tasksService;

        private int _currentUserId;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;

            // Automapper configuration
            AutoMapper.Mapper.CreateMap<Models.NewTask, DomainModel.Task>();
            AutoMapper.Mapper.CreateMap<Models.UpdateTask, DomainModel.Task>();

            // We are simulating the current user. The Current User ID should be retrieved using 
            // the valid security token (to be implemented) that should be present in the HTTP request
            _currentUserId = 1;
        }

        [HttpGet]
        [Route("", Name="GetAllTasks")]
        public IHttpActionResult GetAllTasks()
        {
            var tasksCollection = _tasksService.GetAll();
            return Ok(tasksCollection);
        }

        /// <summary>
        /// Returns all the tasks entities in a particular status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("status/{status}", Name="GetAllTaskFilteredByStatus")]
        public HttpResponseMessage GetAllTasks(DomainModel.TaskStatus status)
        {
            var tasksCollection = _tasksService.GetAll(status);
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, tasksCollection);
            return response;
        }
        
        [HttpGet]
        [Route("{taskID:int}", Name="GetTaskById")]
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
        [Route("", Name="AddNewTask")]
        public IHttpActionResult Add(NewTask model)
        {
            try
            {
                if( ModelState.IsValid)
                {
                    var newTask = Mapper.Map<Models.NewTask, DomainModel.Task>(model);
                    newTask.CreatorUserId = _currentUserId;
                    newTask = _tasksService.Add(newTask);
                    string location = Url.Link("GetTaskById", new { taskID = newTask.ID });
                    return Created(location, newTask);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(InvalidEntityException<DomainModel.Task>)
            {
                // There can be some other business rules that make the new task not valid
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{taskID:int}", Name="UpdateTask")]
        public IHttpActionResult Update(int taskID, UpdateTask model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var taskToUpdate = Mapper.Map<Models.UpdateTask, DomainModel.Task>(model);
                    taskToUpdate.ID = taskID;
                    var updatedTask = _tasksService.Update(taskToUpdate);
                    return Ok(updatedTask);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (InvalidEntityException<DomainModel.Task>)
            {
                // There can be some other business rules that make the new task not valid
                return BadRequest();
            }
            catch(NotFoundEntityException<DomainModel.Task>)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{taskID:int}")]
        public IHttpActionResult Delete(int taskID)
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
