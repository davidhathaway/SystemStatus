using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;
using System.Data.Entity;

namespace SystemStatus.Domain.QueryHandlers
{
    public class SystemStatusQueryHandler : IQueryHandler<SystemStatusQuery, IEnumerable<SystemStatusViewModel>>
    {
        public IEnumerable<SystemStatusViewModel> Handle(SystemStatusQuery query)
        {
            using (var context = new SystemStatusModel())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var qry = context.Systems.Include(x => x.Apps);

                if(query.ParentGroupID.HasValue)
                {
                    qry = qry.Where(x => x.ParentID == query.ParentGroupID.Value);
                }
                else
                {
                    qry = qry.Where(x => x.ParentID == null);
                }

                var systems = qry
                    .Select(x=> new
                    {
                        System = x,
                        Apps = x.Apps.Select(a => new
                        {
                            App = a,
                            LastEvent = a.Events.OrderByDescending(o => o.EventTime).FirstOrDefault()
                        })
                    })
                    .ToList();

                var model = systems.Select(x => new SystemStatusViewModel()
                {
                    SystemGroupID = x.System.SystemGroupID,
                    Name = x.System.Name,
                    AppStatuses = x.Apps.Select(a => new AppStatusViewModel()
                    {
                        AppID = a.App.AppID,
                        AgentName = a.App.AgentName,
                        Description = a.App.Description,
                        LastAppStatus = a.LastEvent != null ? a.LastEvent.AppStatus : AppStatus.None ,
                        LastEventTime = a.LastEvent != null ? a.LastEvent.EventTime : DateTime.MinValue,
                        LastEventValue = a.LastEvent != null ? a.LastEvent.Value : null,
                        Name = a.App.Name,
                        SystemGroupID = a.App.SystemGroupID
                    }).ToArray()
                }).ToList();

                return model;
            }
        }
    }
}
