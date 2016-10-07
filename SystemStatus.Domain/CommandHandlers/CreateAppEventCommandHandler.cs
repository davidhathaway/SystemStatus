using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.Commands
{
    public class CreateAppEventCommandHandler : CommandHandlerBase<CreateAppEventCommand>
    {
        protected override CommandResult<CreateAppEventCommand> Validate(CreateAppEventCommand command)
        {
            CommandResult<CreateAppEventCommand> result = new CommandResult<CreateAppEventCommand>();
          
            if(command.Model == null)
            {
                result.AddError("Model", "Model is null");
            }
           
            return result;
        }

        protected override void OnHandle(CreateAppEventCommand command, CommandResult<CreateAppEventCommand> result)
        {
            using(var db = new SystemStatusModel())
            {
                var appEvent = new AppEvent();
                appEvent.AppID = command.Model.AppID;
                appEvent.AppStatus = command.Model.AppStatus;
                appEvent.EventTime = command.Model.EventTime;
                appEvent.Message = command.Model.Message;
                appEvent.Value = command.Model.Value;
                db.AppEvents.Add(appEvent);
                db.SaveChanges();

                //check system up time.
                var app = db.Apps.FirstOrDefault(x => x.AppID == command.Model.AppID);
                if(app.IsSystemCritical)
                {
                    //if the app is critical then update the system status
                    var system = app.SystemGroup;

                    var systemApps1 = system
                        .Apps
                        .Where(x => x.IsSystemCritical)
                        .Select(x =>x).ToList();

                    var appIDs = systemApps1.Select(x => x.AppID).ToArray();

                    var systemAppEvents = db.AppEvents
                        .Where(x => appIDs.Contains(x.AppID))
                        .GroupBy(x => x.AppID)
                        .Select(x => new { LastEvent = x.OrderByDescending(o=>o.EventTime).FirstOrDefault(), Key = x.Key }).ToList();


                    var systemApps2 = from s in systemApps1
                                      join e in systemAppEvents on s.AppID equals e.Key
                                      select new { LastEvent = e.LastEvent, App = s };

                    var isDown = systemApps2.Any(x => x.LastEvent!=null && x.LastEvent.AppStatus == AppStatus.None);
                    var events = UpdateSystemEvent(system, isDown);
                    if (events.Count() > 0)
                    {
                        db.SystemEvents.AddRange(events);
                        db.SaveChanges();
                    }
                }
            }
        }

        private IEnumerable<SystemEvent> UpdateSystemEvent(SystemGroup group, bool isDown)
        {
            var events = new List<SystemEvent>();

            var lastSystemEvent = group.SystemEvents.OrderByDescending(x => x.EventTime).FirstOrDefault();
            if(lastSystemEvent == null)
            {
                var newSystemEvent = new SystemEvent() { EventTime = DateTime.Now, IsDown = isDown, SystemGroupID = group.SystemGroupID, SystemGroup = group };
                events.Add(newSystemEvent);
            }
            else
            {
                if (lastSystemEvent.IsDown != isDown)
                {
                    //state changed
                    var newSystemEvent = new SystemEvent() { EventTime = DateTime.Now, IsDown = isDown, SystemGroupID = group.SystemGroupID, SystemGroup = group };
                    events.Add(newSystemEvent);

                    //check parent system if this group is critical
                    if(group.IsSystemCritical && group.Parent!=null)
                    {
                        var parentEvents = UpdateSystemEvent(group.Parent, isDown);
                        events.AddRange(parentEvents);
                    }
                }
            }

            return events;

        }
    }
}
