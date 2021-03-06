﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Commands;
using SystemStatus.Domain.Queries;

namespace SystemStatus.Domain.QueryHandlers
{
    public class EditSystemCommandQueryHandler : IQueryHandler<Queries.EditSystemCommandQuery, Domain.Commands.EditSystemCommand>
    {
        public EditSystemCommand Handle(EditSystemCommandQuery query)
        {
            using (var context = new SystemStatusModel())
            {
                var group = context.Systems.FirstOrDefault(x => x.SystemGroupID == query.SystemGroupID);

                if (group != null)
                {
                    var possibleParents = context.Systems
                        .Where(x => x.ParentID == null && x.SystemGroupID != group.SystemGroupID)
                        .ToDictionary(x=>x.SystemGroupID.ToString(), x=>x.Name);

                    return new EditSystemCommand()
                    {
                        Name = group.Name,
                        ParentGroupID = group.ParentID,
                        SystemGroupID = group.SystemGroupID,
                        IsSystemCritical= group.IsSystemCritical,
                        PossibleParentGroups = possibleParents
                    };
                }
                else
                {
                    throw new KeyNotFoundException("SystemGroupID Not Found : " + group.SystemGroupID);
                }
            }
        }
    }
}
