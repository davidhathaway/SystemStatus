using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Commands;

namespace SystemStatus.Domain.Queries
{
    public class EditSystemCommandQuery : IQuery<EditSystemCommand>
    {
        public int SystemGroupID { get; set; }
    }
}
