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
    class ListAppEventsForDrillDownQueryHandler : IQueryHandler<ListAppEventsForDrillDownQuery, AppEventCollectionViewModel>
    {
        public AppEventCollectionViewModel Handle(ListAppEventsForDrillDownQuery query)
        {
            using(var db = new SystemStatusModel())
            {
                var app = db.Apps.First(x => x.AppID == query.AppID);

                var events = db.AppEvents
                    .Include(x=>x.Message)
                    .Where(x => x.AppID == query.AppID)
                    .OrderByDescending(x => x.EventTime)
                    .Take(query.TopN)
                    .Select(x => new AppEventViewModel()
                    {
                        AppEventID = x.AppEventID,
                        AppStatus = x.AppStatus,
                        EventTime = x.EventTime,
                        Message = x.Message == null ? string.Empty : x.Message.Value,
                        Value = x.Value ?? (x.AppStatus == AppStatus.None ? -1 : 1)
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
