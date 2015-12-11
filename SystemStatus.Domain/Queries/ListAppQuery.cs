﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Queries
{
    public class ListAppQuery : IQuery<IEnumerable<App>>
    {
        public int? AppID { get; set; }
        public string AgentName { get; set; }
    }
}
