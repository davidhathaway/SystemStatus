﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemStatus.Domain;

namespace SystemStatus.Domain.ViewModels
{
    public class SystemGroupViewModel
    {
        public int SystemGroupID { get; set; }

        public string Name { get; set; }

        public int? ParentID { get; set; }

        public IEnumerable<int> Children { get; set; }

        public IEnumerable<AppStatusViewModel> Apps { get; set; }
    }

  
    public class AppStatusViewModel
    {
        public int SystemID { get; set; }
        public int AppID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AgentName { get; set; }
        public AppStatus LastAppStatus { get; set; }
        public string LastAppStatusText { get { return Enum.GetName(typeof(AppStatus), this.LastAppStatus); } }
        public decimal? LastEventValue { get; set; }
        public DateTime LastEventTime { get; set; }
    }
}