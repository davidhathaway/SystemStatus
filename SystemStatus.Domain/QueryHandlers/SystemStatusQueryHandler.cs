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
                context.Configuration.ProxyCreationEnabled = true;
                context.Configuration.LazyLoadingEnabled = true;

                var qry = context.Systems
                    .Include(x => x.Apps)
                    .Include(x => x.ChildGroups);

                if (query.ParentGroupID.HasValue)
                {
                    qry = qry.Where(x => x.ParentID == query.ParentGroupID.Value);
                }
                else
                {
                    qry = qry.Where(x => x.ParentID == null);
                }
       
                var model = qry.ToList().Select(x => new SystemStatusViewModel()
                {
                    SystemGroupID = x.SystemGroupID,
                    Name = x.Name,
                    AppStatuses = GetAllApps(x).ToArray()
                }).ToList();

                return model;
            }
        }

        private IEnumerable<AppStatusViewModel> GetAllApps(SystemGroup group)
        {
            List<AppStatusViewModel> allApps = new List<AppStatusViewModel>();

            var appsWithLastEvent = group.Apps.Select(x => new
            {
                App = x,
                LastEvent = x.Events.OrderByDescending(o => o.EventTime).FirstOrDefault()
            }).ToList();

            if (appsWithLastEvent.Count > 0)
            {
                var apps = appsWithLastEvent.Select(a => new AppStatusViewModel()
                {
                    AppID = a.App.AppID,
                    AgentName = a.App.AgentName,
                    Description = a.App.Description,
                    LastAppStatus = a.LastEvent != null ? a.LastEvent.AppStatus : AppStatus.None,
                    LastEventTime = a.LastEvent != null ? a.LastEvent.EventTime : DateTime.MinValue,
                    LastEventValue = a.LastEvent != null ? a.LastEvent.Value : null,
                    Name = a.App.Name,
                    SystemGroupID = a.App.SystemGroupID
                }).ToList();
                allApps.AddRange(apps);
            }

            var childGroups = group.ChildGroups.ToList();

            foreach (var child in childGroups)
            {
                var childApps = GetAllApps(child);
                allApps.AddRange(childApps);
            }

            return allApps;

        }
    }
}
