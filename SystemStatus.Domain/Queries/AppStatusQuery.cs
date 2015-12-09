using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Domain.Queries
{
    public class AppStatusQuery : IQuery<IEnumerable<AppStatusViewModel>>
    {
    }

}
