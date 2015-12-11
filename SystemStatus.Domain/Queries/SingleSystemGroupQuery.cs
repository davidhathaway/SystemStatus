using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Queries
{
    public class SingleSystemGroupQuery : IQuery<ViewModels.SystemGroupViewModel>
    {
        public int SystemGroupID { get; set; }
    }
}
