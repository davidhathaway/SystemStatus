using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Domain.QueryHandlers
{
    public class AppStatusQueryHandler : IQueryHandler<AppStatusQuery, IEnumerable<AppStatusViewModel>>
    {
        public IEnumerable<AppStatusViewModel> Handle(AppStatusQuery query)
        {
            using (var context = new SystemStatusModel())
            {
                context.Configuration.ProxyCreationEnabled = false;
                var apps = context.Apps.Select(x => new
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
                    Description = x.App.Description,
                    MachineName = x.App.MachineName,
                    LastAppStatus = x.Last10Events.Count() > 0 ? x.Last10Events.First().AppStatus : AppStatus.None,
                    LastEventTime = x.Last10Events.Count() > 0 ? x.Last10Events.First().EventTime : DateTime.MinValue,
                    LastEventValue = x.Last10Events.Count() > 0 ? x.Last10Events.First().Value : null
                }).ToList();

                return model;
            }
        }
    }
}
