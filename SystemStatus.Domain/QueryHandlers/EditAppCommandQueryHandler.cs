using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Commands;
using SystemStatus.Domain.Queries;

namespace SystemStatus.Domain.QueryHandlers
{
    public class EditAppCommandQueryHandler : IQueryHandler<EditAppCommandQuery, EditAppCommand>
    {
        public EditAppCommand Handle(EditAppCommandQuery query)
        {
            using (var context = new SystemStatusModel())
            {
                var entity = context.Apps.FirstOrDefault(x => x.AppID == query.AppID);
                if (entity != null)
                {

                    var possibleGroups = context
                        .Systems
                        .ToDictionary(x => x.SystemGroupID.ToString(), x => x.Name);

                    return new Domain.Commands.EditAppCommand()
                    {
                        AppID = entity.AppID,
                        AgentName = entity.AgentName,
                        AppEventHookTypeID = entity.AppEventHookTypeID,
                        Command = entity.AppEventHookTypeID == 2 ? entity.HttpUrl : entity.Command,
                        Description = entity.Description,
                        FastStatusLimit = entity.FastStatusLimit,
                        NormalStatusLimit = entity.NormalStatusLimit,
                        Name = entity.Name,
                        SystemGroupID = entity.SystemGroupID,
                        IsSystemCritical = entity.IsSystemCritical,
                        PossibleSystemGroups = possibleGroups
                    };
                }
                else
                {
                    throw new KeyNotFoundException("ID Not Found : " + entity.AppID);
                }
            }

        }
    }
}
