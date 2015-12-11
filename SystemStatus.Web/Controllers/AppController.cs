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
    public class AppController : ApiController
    {
        private IQueryProcessor queryProcessor;

        public AppController(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        // GET api/<controller>
        public IEnumerable<App> Get()
        {
            var qry = new ListAppQuery();
            var result = queryProcessor.Process(qry);
            return result;
        }

        // GET api/<controller>/5
        public App Get(int id)
        {
            var qry = new SingleAppQuery() { AppID = id };
            var result = queryProcessor.Process(qry);
            return result;
        }

        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<App> GetAllByMachineName(string id)
        {
            var qry = new ListAppQuery() { AgentName = id };
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
            var query = new ListAppEventsForDrillDownQuery() { AppID = id };
            var result = this.queryProcessor.Process(query);
            return result;

        }
    }
}