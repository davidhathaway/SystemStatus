using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Domain.QueryHandlers
{
    public class AppDrillDownQueryHandler : IQueryHandler<AppDrillDownQuery, AppDrilldownViewModel>
    {
        public AppDrilldownViewModel Handle(AppDrillDownQuery query)
        {
            using(var context = new SystemStatusModel())
            {
                var app = context.Apps.First(x => x.AppID == query.AppID);
                return new AppDrilldownViewModel()
                {
                    AppID = app.AppID,
                    Name = app.Name,
                    Description = app.Description,
                    AppEventHooks = app.Hooks.Where(x => x.Active).ToDictionary(x => x.AppEventHookID, x =>  x.Name )
                };
            }
        }
    }
}
