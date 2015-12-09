using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain;

namespace SystemStatus.Agent
{
    public interface IHookHandler
    {
        int AppEventHookTypeID { get; }
        Task<AppEvent> Handle(AppEventHook hook);
    }
}
