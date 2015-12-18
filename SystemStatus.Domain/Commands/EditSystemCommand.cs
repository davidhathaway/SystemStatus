using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class EditSystemCommand : ICommand
    {
        [Required]
        public int SystemGroupID { get; set; }

        public int? ParentGroupID { get; set; }

        public Dictionary<string, string> PossibleParentGroups { get; set; }

        [Display(Name = "Is System Critical")]
        [Required]
        public bool IsSystemCritical { get; set; }

        [Display(Name="Name")]
        [Required]
        public string Name { get; set; }

    }
}
