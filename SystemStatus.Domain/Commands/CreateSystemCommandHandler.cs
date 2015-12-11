using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class CreateSystemCommandHandler : CommandHandlerBase<CreateSystemCommand>
    {
        protected override CommandResult<CreateSystemCommand> Validate(CreateSystemCommand command)
        {
            CommandResult<CreateSystemCommand> result = new CommandResult<CreateSystemCommand>();

            //no mutple names
            using(var context = new SystemStatusModel())
            {
                if(context.Systems.Any(x => x.Name == command.Name))
                {
                    result.Errors.Add("Name", new List<string>() { "Duplicate Name: You must provide a unique Name." });
                }
            }

            return result;
        }

        protected override void OnHandle(CreateSystemCommand command, CommandResult<CreateSystemCommand> result)
        {
            using (var context = new SystemStatusModel())
            {
                SystemGroup system;

                if (command.ParentGroupID.HasValue)
                {
                    system = new SystemGroup()
                    {
                        ParentID = command.ParentGroupID,
                        Name = command.Name
                    };
                }
                else
                {
                    system = new SystemGroup()
                    {
                        Name = command.Name
                    };
                }

                context.Systems.Add(system);

                context.SaveChanges();
            }
        }
    }
}
