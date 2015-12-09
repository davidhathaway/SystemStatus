using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemStatus.Domain;
using SystemStatus.Models;

namespace SystemStatus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            SystemStatusModel context= new SystemStatusModel();

            var apps = context.Apps.Select(x=>new
            { 
                App = x, 
                Last10Events= x.Events
                    .OrderByDescending(e=>e.EventTime)
                    .Select(e=>e.AppStatus)
                    .Take(10)
            }).ToList();

            var model = apps.Select(x => new AppStatusViewModel()
            {
                AppID = x.App.AppID,
                Name = x.App.Name,
                LastAppStatus = x.Last10Events.FirstOrDefault()
            }).ToList();


            return View(model);
        }
    }
}