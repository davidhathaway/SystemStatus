using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SystemStatus.Domain
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}
