using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.ViewModels
{
    public class AppDrilldownViewModel
    {
        public int AppID { get; set; }

        [Display(Name="Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Change Event Hook")]
        public int SelectedAppEventHook { get; set; }

        public Dictionary<int, string> AppEventHooks { get; set; }
    }
}
