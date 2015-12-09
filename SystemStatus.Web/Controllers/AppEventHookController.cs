using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SystemStatus.Domain;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Web.Controllers
{
    public class AppEventHookController : ApiController
    {
        private IQueryProcessor queryProcessor;

        public AppEventHookController(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        // GET api/<controller>
        public IEnumerable<AppEventHook> Get()
        {
            var qry = new ListAppEventHooksQuery();
            var result = queryProcessor.Process(qry);
            return result;
        }

        // GET api/<controller>/5
        public AppEventHook Get(int id)
        {
            var qry = new SingleAppEventHookQuery() { AppEventHookID = id };
            var result = queryProcessor.Process(qry);
            return result;
        }

        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<AppEventHook> GetAllByMachineName(string id)
        {
            var qry = new ListAppEventHooksQuery() { MachineName = id };
            var result = queryProcessor.Process(qry);
            return result;
        }

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        public AppEventCollectionViewModel DrillDown(int id)
        {
            //get last n events
            var query = new ListAppEventsForDrillDownQuery() { AppEventHookID = id };
            var result = this.queryProcessor.Process(query);
            return result;

        }
    }
}