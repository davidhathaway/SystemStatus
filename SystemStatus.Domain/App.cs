using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain
{
    public class App
    {
        public virtual int AppID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AgentName { get; set; }

        public virtual int SystemGroupID { get; set; }
        public virtual SystemGroup SystemGroup { get; set; }
        public virtual ICollection<AppEvent> Events { get; set; }

        //app hook
        public virtual int AppEventHookTypeID { get; set; }
        public virtual AppEventHookType AppEventHookType { get; set; }
        public string Command { get; set; }
        public string Script { get; set; }
        public string HttpUrl { get; set; }
        public bool Active { get; set; }
        public decimal FastStatusLimit { get; set; }
        public decimal NormalStatusLimit { get; set; }

       
        public bool IsSystemCritical { get; set; }


        public App()
        {
            IsSystemCritical = false;
            Events = new List<AppEvent>();
        }
    }
}
