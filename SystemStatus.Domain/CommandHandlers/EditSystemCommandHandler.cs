using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class EditSystemCommandHandler : CommandHandlerBase<EditSystemCommand>
    {
        protected override CommandResult<EditSystemCommand> Validate(EditSystemCommand command)
        {
            CommandResult<EditSystemCommand> result = new CommandResult<EditSystemCommand>();

            //no mutple names
            using(var context = new SystemStatusModel())
            {
                if(context.Systems.Where(x=>x.SystemGroupID!=command.SystemGroupID).Any(x => x.Name == command.Name))
                {
                    result.Errors.Add("Name", new List<string>() { "Duplicate Name: You must provide a unique Name." });
                }
            }

            return result;
        }

        protected override void OnHandle(EditSystemCommand command, CommandResult<EditSystemCommand> result)
        {
            using (var context = new SystemStatusModel())
            {
                var current = context.Systems.FirstOrDefault(x => x.SystemGroupID == command.SystemGroupID);
                if (current != null)
                {
                    current.Name = command.Name;
                    current.ParentID = command.ParentGroupID;
                    current.IsSystemCritical = command.IsSystemCritical;
                    context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException("System not found: " + command.SystemGroupID);
                }
  
            }
        }
    }
}
