using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemStatus.Domain.ViewModels;
using NLog;
namespace SystemStatus.Web.Hubs
{
    public class AppEventHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<AppEventHub>();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void UpdateAppStatus(AppStatusViewModel clientModel)
        {
            AppEventHub.Log("updateAppStatus");
            Clients.All.updateAppStatus(clientModel);
        }

        public static void UpdateAppStatusInternal(AppStatusViewModel clientModel)
        {
            try
            {
                hubContext.Clients.All.updateAppStatus(clientModel);
            }
            catch (Exception ex)
            {
                Log("HUB ERROR: " + ex.ToString());
                throw;
            }
            Log("Updated Hub");
        }

        public static void Log(string message)
        {
            logger.Log(LogLevel.Info, message);
        }
    }
}