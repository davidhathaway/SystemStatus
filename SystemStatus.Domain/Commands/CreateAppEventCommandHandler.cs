using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class CreateAppEventCommandHandler : CommandHandlerBase<CreateAppEventCommand>
    {
        protected override CommandResult<CreateAppEventCommand> Validate(CreateAppEventCommand command)
        {
            CommandResult<CreateAppEventCommand> result = new CommandResult<CreateAppEventCommand>();
          
            if(command.Model == null)
            {
                result.AddError("Model", "Model is null");
            }
           
            return result;
        }

        protected override void OnHandle(CreateAppEventCommand command, CommandResult<CreateAppEventCommand> result)
        {
            using(var db = new SystemStatusModel())
            {
                var appEvent = new AppEvent();
                appEvent.AppID = command.Model.AppID;
                appEvent.AppStatus = command.Model.AppStatus;
                appEvent.EventTime = command.Model.EventTime;
                appEvent.Message = command.Model.Message;
                appEvent.Value = command.Model.Value;
                db.AppEvents.Add(appEvent);
                db.SaveChanges();
            }
        }
    }
}
