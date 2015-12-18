using System;

namespace SystemStatus.Domain
{
    public class SystemEvent
    {
        public virtual int SystemEventID { get; set; }

        public virtual int SystemGroupID { get; set; }

        public virtual SystemGroup SystemGroup { get; set; }

        public DateTime EventTime { get; set; }

        public bool IsDown { get; set; }
    }
}