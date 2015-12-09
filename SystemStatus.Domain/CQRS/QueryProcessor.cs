using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using System.Diagnostics;
namespace SystemStatus.Domain
{
    public class QueryProcessor : IQueryProcessor
    {
        private readonly Container container;

        public QueryProcessor(Container container)
        {
            this.container = container;
        }

        [DebuggerStepThrough]
        public TResult Process<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            //get handler
            //this._container.
            dynamic handler = container.GetInstance(handlerType);

            //invoke handler.
            return (TResult)handler.Handle((dynamic)query);
        }
    }
}
