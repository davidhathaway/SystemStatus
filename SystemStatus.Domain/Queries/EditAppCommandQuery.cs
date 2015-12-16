using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Commands;

namespace SystemStatus.Domain.Queries
{
    public class EditAppCommandQuery : IQuery<EditAppCommand>
    {
        public int AppID { get; set; }
    }
}
