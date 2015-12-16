using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class EditAppCommandHandler : CommandHandlerBase<EditAppCommand>
    {
        protected override CommandResult<EditAppCommand> Validate(EditAppCommand command)
        {
            CommandResult<EditAppCommand> result = new CommandResult<EditAppCommand>();
            using (var context = new SystemStatusModel())
            {
               if(!context.AppEventHookTypes.Any(x => x.AppEventHookTypeID == command.AppEventHookTypeID))
               {
                    result.AddError("AppEventHookTypeID", "Invalid hook type.");
               }

                if (context.Apps.Where(x=>x.AppID!=command.AppID).Any(x => x.Name == command.Name && x.SystemGroupID == command.SystemGroupID))
                {
                    result.Errors.Add("Name", new List<string>() { "Duplicate Name: You must provide a unique Name." });
                }

            }
            return result;
        }

        protected override void OnHandle(EditAppCommand command, CommandResult<EditAppCommand> result)
        {
            using (var context = new SystemStatusModel())
            {
                var current = context.Apps.FirstOrDefault(x => x.AppID == command.AppID);
                if (current != null)
                {
                    current.SystemGroupID = command.SystemGroupID;
                    current.Description = command.Description;
                    current.Name = command.Name;
                    current.AgentName = command.AgentName;
                    current.AppEventHookTypeID = command.AppEventHookTypeID;
                    current.FastStatusLimit = command.FastStatusLimit;
                    current.NormalStatusLimit = command.NormalStatusLimit;
                    switch (command.AppEventHookTypeID)
                    {
                        case 1:
                        case 3:
                        case 4:
                            current.Command = command.Command;
                            break;
                        case 2:
                            current.HttpUrl = command.Command;
                            break;
                    }
                    context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException("AppID not found: " + command.AppID);
                }
            }
        }
    }
}
