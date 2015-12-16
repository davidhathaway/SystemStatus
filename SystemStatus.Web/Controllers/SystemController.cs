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
        private ICommandHandler<EditSystemCommand> editSystemHandler;
        private ICommandHandler<EditAppCommand> editAppHandler;

        public SystemController(IQueryProcessor queryProcessor, 
            ICommandHandler<CreateAppCommand> createAppHandler,
            ICommandHandler<CreateSystemCommand> createSystemHandler,
            ICommandHandler<EditSystemCommand> editSystemHandler,
            ICommandHandler<EditAppCommand> editAppHandler)
        {
            this.queryProcessor = queryProcessor;
            this.createAppHandler = createAppHandler;
            this.createSystemHandler = createSystemHandler;
            this.editSystemHandler = editSystemHandler;
            this.editAppHandler = editAppHandler;
        }

        public ActionResult Index(int id)
        {
            var query = new SingleSystemGroupQuery() { SystemGroupID = id };
            var model = this.queryProcessor.Process(query);

            //system groups
            var systemQuery = new SystemStatusQuery() { ParentGroupID = id };
            model.Children = this.queryProcessor.Process(systemQuery);

            foreach (var item in model.Children)
            {
                item.DrillDownUrl = Url.Action("Index", "System", new { id = item.SystemGroupID });
            }

            return View(model);
        }

        public JsonResult RefreshAll(int id)
        {
            var query = new SingleSystemGroupQuery() { SystemGroupID = id };
            var model = this.queryProcessor.Process(query);

            //system groups
            var systemQuery = new SystemStatusQuery() { ParentGroupID = id };
            model.Children = queryProcessor.Process(systemQuery);

            foreach (var item in model.Children)
            {
                item.DrillDownUrl = Url.Action("Index", "System", new { id = item.SystemGroupID });
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult DrillDownDialog(int id)
        {
            var query = new AppDrillDownQuery() { AppID = id };
            AppDrilldownViewModel model = this.queryProcessor.Process(query);
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
                this.createAppHandler
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
                this.createSystemHandler
                    .Handle(model)
                    .MergeWith(this.ModelState);
            }

            return Json(new
            {
                Success = ModelState.IsValid,
                Errors = ModelState.Errors()
            });
        }

        public PartialViewResult EditSystemDialog(int id)
        {
            var query = new EditSystemCommandQuery() { SystemGroupID = id };
            var model = this.queryProcessor.Process(query);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult EditSystemDialog(EditSystemCommand model)
        {
            if (ModelState.IsValid)
            {
                this.editSystemHandler
                    .Handle(model)
                    .MergeWith(this.ModelState);
            }

            return Json(new
            {
                Success = ModelState.IsValid,
                Errors = ModelState.Errors()
            });
        }

        public PartialViewResult EditAppDialog(int id)
        {
            var query = new EditAppCommandQuery() { AppID = id };
            var model = this.queryProcessor.Process(query);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult EditAppDialog(EditAppCommand model)
        {
            if (ModelState.IsValid)
            {
                this.editAppHandler
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