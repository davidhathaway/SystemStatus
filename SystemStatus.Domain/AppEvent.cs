﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemStatus.Domain
{
    public class AppEvent
    {
        public virtual int AppEventID { get; set; }

        public DateTime EventTime { get; set; }

        public virtual App App { get; set; }

        public virtual int AppID { get; set; }

        public virtual AppStatus AppStatus { get; set; }

        public decimal? Value { get; set; }

        public virtual AppEventMessage Message { get; set; }

        public AppEvent()
        {
            EventTime = DateTime.Now;
        }
    }
}
