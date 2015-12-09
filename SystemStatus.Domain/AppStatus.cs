using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemStatus.Domain
{
    public enum AppStatus
    { 
        None= 0,
        Fast = 1,
        Normal = 2,
        Slow = 3,
        Running = 4
    }
}
