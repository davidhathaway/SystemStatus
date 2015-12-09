using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Domain.Queries
{
    public class ListAppEventsForDrillDownQuery : IQuery<AppEventCollectionViewModel>
    {
        public int AppEventHookID { get; set; }
        public int TopN { get; set; }
        public ListAppEventsForDrillDownQuery()
        {
            TopN = 100;
        }
    }
}
