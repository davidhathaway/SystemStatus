using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class CreateAppCommand : ICommand
    {
        public int SystemGroupID { get; set; }

        [Display(Name="Application Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Application Description")]
        public string Description { get; set; }

        [Display(Name = "Event Hook Type")]
        [Required]
        public int AppEventHookTypeID { get; set; }

        [Display(Name = "Command")]
        public string Command { get; set; }

        [Display(Name = "Agent Machine Name")]
        [Required]
        public string AgentName { get; set; }

        [Display(Name = "Fast Limit")]
        public decimal FastStatusLimit { get; set; }

        [Display(Name = "Normal Limit")]
        public decimal NormalStatusLimit { get; set; }

        [Display(Name = "System Critical")]
        [Required]
        public bool IsSystemCritical { get; set; }

        public Dictionary<string, string> GetAppEventHooks()
        {
            var result = new Dictionary<string, string>();

            result.Add("1", "Ping");
            result.Add("2", "Http");
            result.Add("3", "Service");
            result.Add("4", "SqlServer");

            return result;
        }
    }
}
