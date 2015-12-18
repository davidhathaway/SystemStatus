using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class CreateAppCommandHandler : CommandHandlerBase<CreateAppCommand>
    {
        protected override CommandResult<CreateAppCommand> Validate(CreateAppCommand command)
        {
            CommandResult<CreateAppCommand> result = new CommandResult<CreateAppCommand>();

            using (var context = new SystemStatusModel())
            {
                if (context.Apps.Any(x => x.Name == command.Name && x.SystemGroupID == command.SystemGroupID))
                {
                    result.Errors.Add("Name", new List<string>() { "Duplicate Name: You must provide a unique Name." });
                }
            }

            return result;
        }

        protected override void OnHandle(CreateAppCommand command, CommandResult<CreateAppCommand> result)
        {
            using (var context = new SystemStatusModel())
            {
                var hookType = context.AppEventHookTypes.First(x => x.AppEventHookTypeID == command.AppEventHookTypeID);

                var app = new App() 
                {
                    SystemGroupID = command.SystemGroupID,
                    Description = command.Description,
                    Name = command.Name,
                    AgentName = command.AgentName,
                    AppEventHookTypeID = command.AppEventHookTypeID,
                    FastStatusLimit = command.FastStatusLimit,
                    NormalStatusLimit = command.NormalStatusLimit,
                    Active = true,
                    IsSystemCritical = command.IsSystemCritical
                };

                switch (command.AppEventHookTypeID)
	            {
		            case 1:
                    case 3:
                    case 4:
                        app.Command = command.Command;
                        break;
                    case 2:
                        app.HttpUrl = command.Command;
                        break;
	            }

                context.Apps.Add(app);
                context.SaveChanges();
            }
        }
    }
}
