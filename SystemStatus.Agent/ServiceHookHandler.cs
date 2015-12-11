using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain;
using System.ServiceProcess;

namespace SystemStatus.Agent
{
    public class ServiceHookHandler : BaseHookHandler
    {
        public override int AppEventHookTypeID
        {
            get { return 3; }
        }

        public ServiceHookHandler()
        {
          
        }

        protected override async Task<AppEvent> OnHandle(App app)
        {

           return await Task.Run<AppEvent>(() => {

                ServiceController sc = new ServiceController(app.Command);
                var message = "Service is " + Enum.GetName(typeof(ServiceControllerStatus), sc.Status);
                var appEvent = this.CreateFromApp(app, null);
                appEvent.Message = message;
                appEvent.AppStatus = sc.Status == ServiceControllerStatus.Running ? AppStatus.Running : AppStatus.None;
                return appEvent;
            
            });

        }


     
    }
}
