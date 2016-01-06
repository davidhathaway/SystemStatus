using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.ViewModels
{
    public class SystemStatusViewModel
    {
        public int SystemGroupID { get; set; }
        public string Name { get; set; }
      //public AppStatusViewModel[] AppStatuses { get; set; }
        public string DrillDownUrl { get; set; }

        public DateTime? EventTime { get; set; }
        public bool IsDown { get; set; }

        public SubSystemViewModel[] SubSystems { get; set; }
    }
    public class SubSystemViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public bool IsSystem { get; set; }
        public bool IsSystemCritical { get; set; }
        public AppStatus AppStatus { get; set; }
        public string DrillDownUrl { get; set; }
    }

}
