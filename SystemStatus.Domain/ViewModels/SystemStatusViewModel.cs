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
        public AppStatus[] AppStatuses { get; set; }
        public string DrillDownUrl { get; set; }
    }
}
