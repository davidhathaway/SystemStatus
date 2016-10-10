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

        protected override async Task<AppEvent> OnHandle(App app)
        {
            
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var response = await client.GetAsync(app.HttpUrl);
                sw.Stop();
                AppEvent appEvent = this.CreateFromApp(app, response.IsSuccessStatusCode ? (decimal?)sw.ElapsedMilliseconds : null);

                if (!response.IsSuccessStatusCode)
                {
                    appEvent.Message = new AppEventMessage() { Value = string.Format("Error: {0}", Enum.GetName(response.StatusCode.GetType(), response.StatusCode)) };
                }

                return appEvent;
            }
           
            catch (HttpRequestException httpEx)
            {
                sw.Stop();
                AppEvent appEvent = this.CreateFromApp(app, null);
                appEvent.Message = new AppEventMessage() { Value = string.Format("Http Exception: {0}", httpEx.ToString()) };
                return appEvent;
            }
            catch(Exception ex)
            {
                sw.Stop();
                AppEvent appEvent = this.CreateFromApp(app, null);
                appEvent.Message = new AppEventMessage() { Value = string.Format("Exception: {0}", ex.ToString()) };
                return appEvent;
            }

         

        
        }


     
    }
}
