using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain;

namespace SystemStatus.Agent
{
    public enum HookHandlerTypes
    {
        PingHook,
        HttpHook,
        ServiceHook
    }

    public abstract class BaseHookHandler : IHookHandler
    {
        public abstract int AppEventHookTypeID { get; }

        public Task<AppEvent> Handle(AppEventHook hook)
        {
            if (hook == null || hook.AppEventHookTypeID == 0)
            {
                throw new ArgumentException("You must pass a valid AppEventHook.", "hook");
            }
            if (hook.AppEventHookTypeID != AppEventHookTypeID)
            {
                throw new ArgumentException("Unexpected AppEventHookType, expected " + AppEventHookTypeID.ToString() + " got : " + hook.AppEventHookTypeID.ToString(), "hook");
            }

            try
            {
                var appEvent = OnHandle(hook);
                return appEvent;
            }
            catch (Exception ex)
            {
                var appEvent = CreateFromHook(hook, null);
                appEvent.Message = string.Format("Exception: {0}", ex.ToString());
                return Task.Run<AppEvent>(() => { return appEvent; });
            }
        }

        protected abstract Task<AppEvent> OnHandle(AppEventHook hook);

        protected AppEvent CreateFromHook(AppEventHook hook, decimal? responseValue)
        {
            var appEvent = new AppEvent() 
            { 
                AppID = hook.AppID,
                FromAppEventHookID = hook.AppEventHookID,
                EventTime = DateTime.Now,
                Value = responseValue 
            };

            if (responseValue.HasValue)
            {
                if (responseValue < hook.FastStatusLimit)
                {
                    appEvent.AppStatus = AppStatus.Fast;
                }
                else if (responseValue < hook.NormalStatusLimit)
                {
                    appEvent.AppStatus = AppStatus.Normal;
                }
                else
                {
                    appEvent.AppStatus = AppStatus.Slow;
                }
            }
            else
            {
                appEvent.AppStatus = AppStatus.None;
            }

            return appEvent;
        }
    }
}
