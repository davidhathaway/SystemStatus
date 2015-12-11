using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemStatus.Domain.Queries;

namespace SystemStatus.Domain.QueryHandlers
{
    public class ListAppQueryHandler : IQueryHandler<ListAppQuery, IEnumerable<App>>
    {
        public IEnumerable<App> Handle(ListAppQuery query)
        {
            using (var context = new SystemStatusModel())
            {
                var appQry = context.Apps.AsNoTracking().AsQueryable();

                if (query.AppID.HasValue)
                {
                    appQry = appQry.Where(x => x.AppID == query.AppID.Value);
                }

                if (!string.IsNullOrWhiteSpace(query.AgentName))
                {
                    appQry = appQry.Where(x => x.AgentName == query.AgentName);
                }

                return appQry.ToList();
            }
        }
    }
}
