﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemStatus.Domain;

namespace SystemStatus.Agent
{

    public class AgentWorker : TimerWorker
    {
        private static IHookHandler[] _handlers;

        public string Url { get; set; }

        public string MachineName { get; set; }

        public Action<Exception> LogErrors { get; set; }

        public AgentWorker(int interval, string url, HookHandlerTypes[] handlers, string machineName, Action<Exception> logErrors) : base(interval)
        {
            this.MachineName = machineName;
            this.Url = url;
            this.LogErrors = logErrors;
            _handlers = GetHandlers(handlers).ToArray();
        }

        private IEnumerable<IHookHandler> GetHandlers(HookHandlerTypes[] handlers)
        {
            foreach (var item in handlers)
            {
                switch (item)
                {
                    case HookHandlerTypes.PingHook: yield return new PingHookHandler(); break;
                    case HookHandlerTypes.HttpHook: yield return new HttpHookHandler(); break;
                    case HookHandlerTypes.ServiceHook: yield return new ServiceHookHandler(); break;
                    case HookHandlerTypes.SqlServerHook: yield return new SqlServerHookHandler(); break;
                }
            }
        }

        protected override void Tick()
        {     
            AsyncHelper.RunSync(() => DoWork());
        }

        private async Task DoWork()
        {
            //do work
            try
            {
                var pingHooks = await GetApps();
                foreach (var hook in pingHooks.AsParallel())
                {
                    IHookHandler handler = _handlers.FirstOrDefault(x => x.AppEventHookTypeID == hook.AppEventHookTypeID);
                    if (handler != null)
                    {
                        try
                        {
                            await RunHookAsync(hook, handler);
                        }
                        catch (Exception ex)
                        {
                            LogErrors(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrors(ex);
                throw ex;
            }
        }

        private async Task<IEnumerable<App>> GetApps()
        {
            //GetAllByMachineName

            using (var client = new HttpClient())
            {
                // New code: //http://localhost:63592/api/AppEventHook/
                client.BaseAddress = new Uri(this.Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var getUrl = "api/App/GetAllByMachineName/" + MachineName;

                HttpResponseMessage response = await client.GetAsync(getUrl);
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<App> content = await response.Content.ReadAsAsync<IEnumerable<App>>();
                    return content;
                }
            }

            return Enumerable.Empty<App>().ToList();
        }

        private async Task PostAppEvent(AppEvent appEvent)
        {
            using (var client = new HttpClient())
            {
                // New code: //http://localhost:63592/api/AppEventHook/
                client.BaseAddress = new Uri(this.Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/AppEvent/", appEvent);
                if (response.IsSuccessStatusCode)
                {
                    Uri appEventUrl = response.Headers.Location;
                }
            }
        }

        private async Task RunHookAsync(App app, IHookHandler handler)
        {
            var appEvent = await handler.Handle(app);
            await PostAppEvent(appEvent);
        }

    }

    internal static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
          TaskFactory(CancellationToken.None,
                      TaskCreationOptions.None,
                      TaskContinuationOptions.None,
                      TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
              .StartNew<Task<TResult>>(func)
              .Unwrap<TResult>()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
              .StartNew<Task>(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }
    }

    public abstract class TimerWorker : IDisposable
    {
        private bool _disposed;

        protected Timer _timer;

        private bool _started;

        private ManualResetEvent _stopping;

        public int Interval { get; set; }

        public TimerWorker(int interval)
        {
            Interval = interval;
            _started = false;
            _stopping = null;
        }

        public void Start()
        {
            if (!_started && _timer == null)
            {
                _timer = new Timer((o) =>
                {
                    if (_stopping != null)
                    {
                        StopTick();
                    }
                    else
                    {
                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        Tick();
                        _timer.Change(Interval, Timeout.Infinite);
                    }

                }, null, 0, this.Interval);

                _started = true;
            }
        }

        protected abstract void Tick();

        public void Stop()
        {
            if (_started)
            {
                _stopping = new ManualResetEvent(false);
                _stopping.WaitOne(); //wait until timer has stopped.
                _stopping = null;
                _started = false;
            }
        }

        private void StopTick()
        {
            if (_timer != null && _stopping != null && _started)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _stopping.Set();
            }
        }

        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TimerWorker()
        {
            Dispose(false);
        }
    }
}
