using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Domain.QueryHandlers
{
    public class SingleAppStatusQueryHandler : IQueryHandler<SingleAppStatusQuery, AppStatusViewModel>
    {
        public AppStatusViewModel Handle(SingleAppStatusQuery query)
        {
            using (var context = new SystemStatusModel())
            {
                context.Configuration.ProxyCreationEnabled = false;

                var apps = context.Apps.Where(x=>x.AppID == query.AppID).Select(x => new
                {
                    App = x,
                    Last10Events = x.Events
                        .OrderByDescending(e => e.EventTime)
                        .Take(10)
                }).ToList();

                var model = apps.Select(x => new AppStatusViewModel()
                {
                    AppID = x.App.AppID,
                    Name = x.App.Name,
                    LastAppStatus = x.Last10Events.Count() > 0 ? x.Last10Events.First().AppStatus : AppStatus.None,
                    LastEventTime = x.Last10Events.Count() > 0 ? x.Last10Events.First().EventTime : DateTime.MinValue,
                    LastEventValue = x.Last10Events.Count()>0 ? x.Last10Events.First().Value : null,
                    SystemGroupID = x.App.SystemGroupID,
                    AgentName = x.App.AgentName,
                    Description = x.App.Description
                    
                }).ToList();

                return model.FirstOrDefault();
            }
        }
    }
}
