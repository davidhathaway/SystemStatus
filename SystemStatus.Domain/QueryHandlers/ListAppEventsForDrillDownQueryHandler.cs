using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Queries;
using SystemStatus.Domain.ViewModels;

namespace SystemStatus.Domain.QueryHandlers
{
    class ListAppEventsForDrillDownQueryHandler : IQueryHandler<ListAppEventsForDrillDownQuery, AppEventCollectionViewModel>
    {
        public AppEventCollectionViewModel Handle(ListAppEventsForDrillDownQuery query)
        {
            using(var db = new SystemStatusModel())
            {
                var hook = db.AppEventHooks.First(x => x.AppEventHookID == query.AppEventHookID);

                var events = db.AppEvents
                    .Where(x => x.FromAppEventHookID == query.AppEventHookID)
                    .OrderByDescending(x=>x.EventTime)
                    .Take(query.TopN)
                    .Select(x => new AppEventViewModel() { 
                        AppEventID= x.AppEventID,
                        AppStatus =x.AppStatus,
                        EventTime = x.EventTime,
                        Message =x.Message,
                        Value = x.Value 

                    })
                    .ToList();

                var result = new AppEventCollectionViewModel();
                result.AppEventHookID = hook.AppEventHookID;
                result.AppID = hook.AppID;
                result.Events = events;
                result.MaxValue = ((int)hook.NormalStatusLimit) * 2;
                result.MinValue = 0;

                return result;
            }
        }
    }
}
