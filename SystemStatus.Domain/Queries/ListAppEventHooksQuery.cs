using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Queries
{
    public class ListAppEventHooksQuery : IQuery<IEnumerable<AppEventHook>>
    {
        public int? AppEventHookID { get; set; }
        public int? AppID { get; set; }
        public int? AppEventHookTypeID { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public string MachineName { get; set; }
        public ListAppEventHooksQuery()
        {
            Active = true;
        }
    }
}
