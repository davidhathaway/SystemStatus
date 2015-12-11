using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain
{
    public class SystemGroup
    {
        public virtual int SystemGroupID { get; set; }
        public string Name { get; set; }

        public virtual int? ParentID { get; set; }

        [ForeignKey("ParentID")]
        public virtual SystemGroup Parent { get; set; }
        public virtual ICollection<SystemGroup> ChildGroups { get; set; }

        public virtual ICollection<App> Apps { get; set; }

        public SystemGroup()
        {
            ChildGroups = new List<SystemGroup>();
            Apps = new List<App>();
        }
    }
}
