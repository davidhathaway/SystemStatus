﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;
using System.Data.Entity;

namespace SystemStatus.Domain.QueryHandlers
{
    public class SingleSystemGroupQueryHandler : IQueryHandler<Queries.SingleSystemGroupQuery, ViewModels.SystemGroupViewModel>
    {
        public SystemGroupViewModel Handle(SingleSystemGroupQuery query)
        {

            using (var context = new SystemStatusModel())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var group = context.Systems.Where(x => x.SystemGroupID == query.SystemGroupID)
                    .Include(x=>x.Apps)
                    .FirstOrDefault();

                if (group != null)
                {
                    var groupApps = context.Apps.Where(x=>x.SystemGroupID == query.SystemGroupID).Select(x => new
                        {
                            App = x,
                            LastEvent = x.Events.OrderByDescending(e => e.EventTime).FirstOrDefault()
                        }).ToList();

                    var apps = groupApps.Select(x => new AppStatusViewModel()
                    {
                        SystemGroupID = x.App.SystemGroupID,
                        AppID = x.App.AppID,
                        Name = x.App.Name,
                        Description = x.App.Description,
                        AgentName = x.App.AgentName,
                        LastAppStatus = x.LastEvent !=null ? x.LastEvent.AppStatus : AppStatus.None,
                        LastEventTime = x.LastEvent != null  ? x.LastEvent.EventTime : DateTime.MinValue,
                        LastEventValue = x.LastEvent != null ? x.LastEvent.Value : null
                    }).ToList();



                    return new SystemGroupViewModel()
                    {
                        Apps = apps,
                        Name = group.Name,
                        ParentID = group.ParentID,
                        SystemGroupID = group.SystemGroupID
                    };
                }
                else
                {
                    return null;
                }
            }

        }
    }
}
