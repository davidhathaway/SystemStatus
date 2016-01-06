using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Agent.WindowsService
{
    public partial class SystemStatusAgentService : ServiceBase
    {
        private AgentWorker _worker;

        public string Url { get; set; }
        
        public int Interval { get; set; }

        public HookHandlerTypes[] Handlers { get; set; }

        public SystemStatusAgentService()
        {
            InitializeComponent();

            this.EventLog.Source = "System Status Agent";
            this.EventLog.Log = "";



            //create event log source
            ((System.ComponentModel.ISupportInitialize)(this.EventLog)).BeginInit();
            if (!EventLog.SourceExists(this.EventLog.Source))
            {
                EventLog.CreateEventSource(this.EventLog.Source, this.EventLog.Log);
            }
            ((System.ComponentModel.ISupportInitialize)(this.EventLog)).EndInit();
        }

        protected override void OnStart(string[] args)
        {
            DoStart();
        }

        protected override void OnStop()
        {
            DoStop();
        }

        private void Log(Exception ex)
        {
            if (this.EventLog != null)
            {
                this.EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
            }
        }

        private void Log(string message)
        {
            if (this.EventLog != null)
            {
                this.EventLog.WriteEntry(message, EventLogEntryType.Information);
            }
        }

        internal void DoStart()
        {
            try
            {
                ReadConfig();
            }
            catch (Exception ex)
            {
                Log(ex);
                this.Stop();
                return;//stop
            }

            try
            {
                if (_worker == null)
                {
                    var handlersToString = this.Handlers.Select(x => x.ToString()).ToArray();
                    var logMsg = string.Format("Starting Agent Worker: Interval {0}, Url {1}, Handlers: {2}", Interval, Url, string.Join(",", handlersToString));
                    Log(logMsg);
                    _worker = new AgentWorker(Interval, Url, Handlers, Environment.MachineName,
                        (ex)=> 
                        {
                            Log(ex);
                        }
                    );
                }
                _worker.Start();
                Log("Worker Started.");
            }
            catch (Exception ex)
            {
                Log(ex);
                this.Stop();
            }
        }

        internal void DoStop()
        {
            if (_worker != null)
            {
                Log("Stopping Worker.");
                _worker.Stop();
                Log("Worker Stopped.");
            }
        }

        public void ReadConfig()
        {

            foreach (string key in ConfigurationManager.AppSettings.Keys)
            {
                string value = ConfigurationManager.AppSettings[key].ToString();

                if (value == null)
                {
                    continue;
                }
                switch (key)
                {      
                    case "ServerUrl":
                        {
                            Url = value;
                        }
                        break;
                    case "Interval":
                        {
                            int parsed = 0;
                            if (int.TryParse(value, out parsed))
                            {
                                this.Interval = parsed;
                            }
                        }
                        break;
                    case "HookHandlers":
                        {
                            var split = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            List<HookHandlerTypes> handlerTypes = new List<HookHandlerTypes>();

                            foreach (var s in split)
                            {
                                HookHandlerTypes parsed;
                                if (Enum.TryParse<HookHandlerTypes>(s, out parsed))
                                {
                                    handlerTypes.Add(parsed);
                                }
                            }
                            this.Handlers = handlerTypes.ToArray();
                        }
                        break;


                }
            }
        }
    }
}
