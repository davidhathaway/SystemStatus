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
        private ICommandHandler<CreateSystemCommand> createSystemHandler;

        public HomeController(IQueryProcessor queryProcessor, ICommandHandler<CreateSystemCommand> createSystemHandler)
        {
            this.queryProcessor = queryProcessor;
            this.createSystemHandler = createSystemHandler;
        }

        public ActionResult Index()
        {
            var query = new SystemStatusQuery();
            var model = queryProcessor.Process(query).ToArray();

            foreach (var item in model)
            {
                item.DrillDownUrl = Url.Action("Index", "System", new { id = item.SystemGroupID });
            }

            return View(model);
        }

        public JsonResult RefreshAll()
        {
            var query = new SystemStatusQuery();
            var model = queryProcessor.Process(query).ToArray();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        

        public PartialViewResult CreateSystemDialog()
        {
            var model = new CreateSystemCommand();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult CreateSystemDialog(CreateSystemCommand model)
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
    }
}
