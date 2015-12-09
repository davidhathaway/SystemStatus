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
    public class HomeController : Controller
    {
        private IQueryProcessor queryProcessor;
        private ICommandHandler<CreateAppCommand> createAppHandler;

        public HomeController(IQueryProcessor queryProcessor, ICommandHandler<CreateAppCommand> createAppHandler)
        {
            this.queryProcessor = queryProcessor;
            this.createAppHandler = createAppHandler;
        }

        public ActionResult Index()
        {
            var query = new AppStatusQuery();
            var model = queryProcessor.Process(query).ToArray();
            return View(model);
        }

        public JsonResult RefreshAll()
        {
            var query = new AppStatusQuery();
            var model = queryProcessor.Process(query).ToArray();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult DrillDownDialog(int id)
        {
            var query = new AppDrillDownQuery() { AppID = id };
            AppDrilldownViewModel model = queryProcessor.Process(query);
            return PartialView(model);
        }

        public PartialViewResult CreateAppDialog()
        {
            var model = new CreateAppCommand();
            model.MachineName = Environment.MachineName;
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
    }
}
