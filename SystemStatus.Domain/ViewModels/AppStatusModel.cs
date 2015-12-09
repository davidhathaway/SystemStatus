using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemStatus.Domain;

namespace SystemStatus.Domain.ViewModels
{
    public class AppStatusViewModel
    {
        public int AppID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MachineName { get; set; }
        public AppStatus LastAppStatus { get; set; }
        public string LastAppStatusText { get { return Enum.GetName(typeof(AppStatus), this.LastAppStatus); } }
        public decimal? LastEventValue { get; set; }
        public DateTime LastEventTime { get; set; }
    }
}