using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Domain.Queries
{
    public class SystemStatusQuery : IQuery<IEnumerable<SystemStatusViewModel>>
    {
        public int? ParentGroupID { get; set; }
    }
}
