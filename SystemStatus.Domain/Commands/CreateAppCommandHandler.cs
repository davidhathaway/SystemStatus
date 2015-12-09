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

            

            return result;
        }

        protected override void OnHandle(CreateAppCommand command, CommandResult<CreateAppCommand> result)
        {
            using (var context = new SystemStatusModel())
            {
                var app = new App() 
                { 
                    Description = command.Description,
                    Name = command.Name,
                    MachineName = command.MachineName
                };

                var hookType = context.AppEventHookTypes.First(x=>x.AppEventHookTypeID == command.AppEventHookTypeID);

                var appHook = new AppEventHook() { 
                    AppEventHookTypeID = command.AppEventHookTypeID,
                    FastStatusLimit = command.FastStatusLimit,
                    NormalStatusLimit = command.NormalStatusLimit,
                    Name = command.Name + ": " + hookType.Name,

                    Active = true
                };

                switch (command.AppEventHookTypeID)
	            {
		            case 1:
                    case 3:
                        appHook.Command = command.Command;
                        break;
                    case 2:
                        appHook.HttpUrl = command.Command;
                        break;
	            }

                app.Hooks.Add(appHook);
                context.Apps.Add(app);
                context.SaveChanges();
            }
        }
    }
}
