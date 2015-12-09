using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SystemStatus.Domain;
using SystemStatus.Domain.Commands;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;
using SystemStatus.Web.Hubs;

namespace SystemStatus.Web.Controllers
{
    public class AppEventController : ApiController
    {
        private CreateAppEventCommandHandler createCommandHandler;
        private IQueryProcessor queryProcessor;

        public AppEventController(CreateAppEventCommandHandler createCommandHandler, IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
            this.createCommandHandler = createCommandHandler;
        }

        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]CreateAppEventModel value)
        {
            CreateAppEventCommand command = new CreateAppEventCommand();
            command.Model = value;
            var result = createCommandHandler.Handle(command);
            if(result.Success)
            {
                UpdateAppStatus(value.AppID);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                //return Request.CreateResponse(HttpStatusCode.InternalServerError, modelOfFailedIds);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private void UpdateAppStatus(int appID)
        {
            //update hub status
            SingleAppStatusQuery appQuery = new SingleAppStatusQuery() { AppID = appID };
            var appStatusResult = queryProcessor.Process(appQuery);
            AppEventHub.UpdateAppStatusInternal(appStatusResult);
        }

        // PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}