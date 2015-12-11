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

                var systems = context.Systems
                    .Include(x=>x.Apps)
                    .Where(x=>x.ParentID == null)
                    .Select(x=> new
                    {
                        System = x,
                        Apps = x.Apps.Select(a => new
                        {
                            AppID = a.AppID,
                            LastEvent = a.Events.OrderByDescending(o => o.EventTime).FirstOrDefault()
                        })
                    })
                    .ToList();

                var model = systems.Select(x => new SystemStatusViewModel()
                {
                    SystemGroupID = x.System.SystemGroupID,
                    Name = x.System.Name,
                    AppStatuses = x.Apps.Select(a=> a.LastEvent == null ? AppStatus.None : a.LastEvent.AppStatus).ToArray()
                }).ToList();

                return model;
            }
        }
    }
}
