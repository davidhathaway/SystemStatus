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
                var app = db.Apps.First(x => x.AppID == query.AppID);

               var events = app.Events
                    .OrderByDescending(x => x.EventTime)
                    .Take(query.TopN)
                    .Select(x => new AppEventViewModel()
                    {
                        AppEventID = x.AppEventID,
                        AppStatus = x.AppStatus,
                        EventTime = x.EventTime,
                        Message = x.Message,
                        Value = x.Value

                    })
                    .ToList();

                var result = new AppEventCollectionViewModel();
                result.AppID = app.AppID;
                result.Events = events;
                result.MaxValue = ((int)app.NormalStatusLimit) * 2;
                result.MinValue = 0;
                return result;
            }
        }
    }
}
