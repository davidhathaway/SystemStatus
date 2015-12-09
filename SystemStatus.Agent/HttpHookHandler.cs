using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain;

namespace SystemStatus.Agent
{
    public class HttpHookHandler : BaseHookHandler
    {
        private HttpClient client;

        public override int AppEventHookTypeID
        {
            get { return 2; }
        }

        public HttpHookHandler()
        {
            client = new HttpClient();
        }

        protected override async Task<AppEvent> OnHandle(AppEventHook hook)
        {
            
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var response = await client.GetAsync(hook.HttpUrl);
                sw.Stop();
                AppEvent appEvent = this.CreateFromHook(hook, response.IsSuccessStatusCode ? (decimal?)sw.ElapsedMilliseconds : null);

                if (response.IsSuccessStatusCode)
                {
                    appEvent.Message = string.Format("Elapsed Milliseconds: {0}", sw.ElapsedMilliseconds);
                }
                else
                {
                    appEvent.Message = string.Format("Error: {0}", Enum.GetName(response.StatusCode.GetType(), response.StatusCode));
                }

                return appEvent;
            }
            catch (HttpRequestException ex)
            {
                sw.Stop();
                AppEvent appEvent = this.CreateFromHook(hook, null);
                appEvent.Message = string.Format("Http Exception: {0}", ex.ToString());
                return appEvent;
            }

         

        
        }


     
    }
}
