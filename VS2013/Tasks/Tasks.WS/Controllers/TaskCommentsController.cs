using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Tasks.WS.Controllers
{
    [RoutePrefix("v1/tasks/comments")]
    public class TaskCommentsController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id:int}")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
