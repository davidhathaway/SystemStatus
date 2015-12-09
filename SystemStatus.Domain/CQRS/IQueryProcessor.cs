using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemStatus.Domain
{
    public interface IQueryProcessor
    {
        TResult Process<TResult>(IQuery<TResult> query);
    }
}
