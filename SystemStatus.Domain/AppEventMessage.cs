using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain
{
    public class AppEventMessage
    {
        public virtual int AppEventMessageID { get; set; }

        public string Value { get; set; }
    }
}
