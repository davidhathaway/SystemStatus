using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemStatus.Domain;
using SystemStatus.Domain.Commands;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;
using SystemStatus.Web.Models;
using SystemStatus.Web.Common;

namespace SystemStatus.Web.Controllers
{
    public class SystemController : Controller
    {
        private IQueryProcessor queryProcessor;
        private ICommandHandler<CreateAppCommand> createAppHandler;
        private ICommandHandler<CreateSystemCommand> createSystemHandler;

        public SystemController(IQueryProcessor queryProcessor, 
            ICommandHandler<CreateAppCommand> createAppHandler,
            ICommandHandler<CreateSystemCommand> createSystemHandler)
        {
            this.queryProcessor = queryProcessor;
            this.createAppHandler = createAppHandler;
            this.createSystemHandler = createSystemHandler;
        }

        public ActionResult Index(int id)
        {
            var query = new SingleSystemGroupQuery() { SystemGroupID = id };
            var model = queryProcessor.Process(query);
            return View(model);
        }

        public JsonResult RefreshAll(int id)
        {
            var query = new SingleSystemGroupQuery() { SystemGroupID = id };
            var model = queryProcessor.Process(query);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

     

        public PartialViewResult DrillDownDialog(int id)
        {
            var query = new AppDrillDownQuery() { AppID = id };
            AppDrilldownViewModel model = queryProcessor.Process(query);
            return PartialView(model);
        }

        public PartialViewResult CreateAppDialog(int id)
        {
            var model = new CreateAppCommand() { SystemGroupID = id };
            model.AgentName = Environment.MachineName;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CreateAppDialog(CreateAppCommand model)
        {
            if (ModelState.IsValid)
            {
                createAppHandler
                    .Handle(model)
                    .MergeWith(this.ModelState);
            }

            return Json(new
            {
                Success = ModelState.IsValid,
                Errors = ModelState.Errors()
            });
        }

        public PartialViewResult CreateSubSystemDialog(int id)
        {
            var model = new CreateSystemCommand() { ParentGroupID = id };
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CreateSubSystemDialog(CreateSystemCommand model)
        {
            if (ModelState.IsValid)
            {
                createSystemHandler
                    .Handle(model)
                    .MergeWith(this.ModelState);
            }

            return Json(new
            {
                Success = ModelState.IsValid,
                Errors = ModelState.Errors()
            });
        }
    }
}