using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain
{
    public class App
    {
        public virtual int AppID { get;set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public string MachineName { get; set; }
        public virtual ICollection<AppEvent> Events { get; set; }
        public virtual ICollection<AppEventHook> Hooks { get; set; }
        public App()
        {
            Events = new List<AppEvent>();
            Hooks = new List<AppEventHook>();
        }
    }
}
