using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain;

namespace SystemStatus.Agent
{

    public class PingHookHandler : BaseHookHandler
    {
        private Ping ping;
        private PingOptions options;
        private byte[] buffer;
        private int timeout = 200;

        public override int AppEventHookTypeID
        {
            get { return 1; }
        }

        public PingHookHandler()
        {
            ping = new Ping();
            //ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
            options = new PingOptions();
            options.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            buffer = Encoding.ASCII.GetBytes(data);   
        }

        protected override async Task<AppEvent> OnHandle(AppEventHook hook)
        {
            var hostName = hook.Command;

            PingReply reply = await ping.SendPingAsync(hostName, timeout, buffer, options);

            AppEvent appEvent = this.CreateFromHook(hook, reply.Status == IPStatus.Success ? (decimal?)reply.RoundtripTime : null);

            if (reply.Status == IPStatus.Success)
            {
                appEvent.Message = string.Format("RoundtripTime (ms): {0}", reply.RoundtripTime);
            }
            else
            {
                appEvent.Message = string.Format("Error: {0}, RoundtripTime (ms): {1}", Enum.GetName(typeof(IPStatus), reply.Status), reply.RoundtripTime);
            }

            return appEvent;
        }
    }
}
