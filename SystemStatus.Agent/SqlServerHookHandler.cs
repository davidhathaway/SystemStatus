using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain;

namespace SystemStatus.Agent
{
    public class SqlServerHookHandler : BaseHookHandler
    {
        public override int AppEventHookTypeID
        {
            get
            {
                return 4;
            }
        }

        public SqlServerHookHandler()
        {

        }

        protected override async Task<AppEvent> OnHandle(App app)
        {
            var connectionString = app.Command;

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                using (var connectionTest = new SqlConnection(connectionString))
                {
                    await connectionTest.OpenAsync();
                    sw.Stop();
                }

                AppEvent appEvent = this.CreateFromApp(app, (decimal?)sw.ElapsedMilliseconds);
                return appEvent;
            }
            catch (Exception ex)
            {
                sw.Stop();
                AppEvent appEvent = this.CreateFromApp(app, null);
                appEvent.Message = new AppEventMessage() { Value = string.Format("Exception: {0}", ex.ToString()) };
                return appEvent;
            }
        }
    }
}
