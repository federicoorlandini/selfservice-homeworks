using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Tasks.WS.Controllers
{
    [RoutePrefix("v1/tasks/comments")]
    public class LogWorksController : ApiController
    {
        [HttpGet]
        [Route("~/v1/tasks/{taskId:int}/comments")]
        public IHttpActionResult GetAllCommentsForTask(int taskId)
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        public IHttpActionResult Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        public IHttpActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
