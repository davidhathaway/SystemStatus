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
                    SubSystems = GetSubSystems(x).ToArray()
                }).ToList();

                //set is down
                foreach (var item in model)
                {
                    item.IsDown = item.SubSystems.Any(x => x.IsSystemCritical && x.AppStatus == AppStatus.None);
                }
 
                return model;
            }
        }

        private IEnumerable<SubSystemViewModel> GetSubSystems(SystemGroup group)
        {

            List<SubSystemViewModel> results = new List<SubSystemViewModel>();

            var appsWithLastEvent = group.Apps.Select(x => new
            {
                App = x,
                LastEvent = x.Events.OrderByDescending(o => o.EventTime).FirstOrDefault()
            }).ToList();

            var appStatus = appsWithLastEvent.Select(x => new SubSystemViewModel()
            {
                ID = x.App.AppID,
                AppStatus = x.LastEvent == null ? AppStatus.None : x.LastEvent.AppStatus,
                IsSystem = false,
                Text = x.App.Name,
                IsSystemCritical = x.App.IsSystemCritical
            });

            results.AddRange(appStatus);

            var systemsWithLastEvent = group.ChildGroups.Select(x => new {
                System = x,
                LastEvent = x.SystemEvents.OrderByDescending(o=>o.EventTime).FirstOrDefault()
            }).ToList();

            var systemStatus = systemsWithLastEvent.Select(x => new SubSystemViewModel() {
                AppStatus = x.LastEvent == null ? AppStatus.None : (x.LastEvent.IsDown ? AppStatus.None : AppStatus.Fast),
                ID = x.System.SystemGroupID,
                IsSystem = true,
                Text = x.System.Name,
                IsSystemCritical = x.System.IsSystemCritical
            });

            results.AddRange(systemStatus);

            return results;

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
