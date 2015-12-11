using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class CreateSystemCommand : ICommand
    {
        public int? ParentGroupID { get; set; }

        [Display(Name="Name")]
        [Required]
        public string Name { get; set; }

    }
}
