using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Queries;

namespace SystemStatus.Domain.QueryHandlers
{
    public class SingleAppEventHooksQueryHandler : IQueryHandler<SingleAppEventHookQuery, AppEventHook>
    {
        public AppEventHook Handle(SingleAppEventHookQuery query)
        {
            using(var context = new SystemStatusModel())
            {
                context.Configuration.ProxyCreationEnabled = false;
                var qry = context.AppEventHooks.AsQueryable();



                if(query.AppEventHookID.HasValue)
                {
                    qry = qry.Where(x => x.AppEventHookID == query.AppEventHookID.Value);
                }

                if (query.AppID.HasValue)
                {
                    qry = qry.Where(x => x.AppID == query.AppID.Value);
                }

                if (query.AppEventHookTypeID.HasValue)
                {
                    qry = qry.Where(x => x.AppEventHookTypeID == query.AppEventHookTypeID.Value);
                }

                if (query.Active.HasValue)
                {
                    qry = qry.Where(x => x.Active == query.Active.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.Name))
                {
                    qry = qry.Where(x => x.Name.Contains(query.Name));
                }

                if (!string.IsNullOrWhiteSpace(query.MachineName))
                {
                    qry = qry.Where(x => x.App.MachineName.Equals(query.MachineName));
                }

                return qry.FirstOrDefault();
            }
        }
    }
}
