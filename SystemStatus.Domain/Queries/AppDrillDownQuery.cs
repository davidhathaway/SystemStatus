using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Domain.Queries
{
    public class AppDrillDownQuery : IQuery<AppDrilldownViewModel>
    {
        public int AppID { get; set; }
    }
}
