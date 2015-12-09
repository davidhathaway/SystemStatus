using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SystemStatus.Domain
{

    public interface IRepository<T> : IRepositoryWithTypeId<T, int>
    {

    }

    public interface IRepositoryGuid<T> : IRepositoryWithTypeId<T, Guid>
    {

    }

    public interface IRepositoryWithTypeId<T, TId>
    {
        IEnumerable<T> FindAll();

        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate);

        T Get(TId id);

        T SaveOrUpdate(T entity);

        void Delete(T entity);

        void Delete(TId id);
    }
}
