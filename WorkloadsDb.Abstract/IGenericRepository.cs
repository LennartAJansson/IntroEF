using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkloadsDb.Model;

namespace WorkloadsDb.Abstract
{
    public interface IGenericRepository<TEntity> where TEntity : class, IPOCOClass
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                 string includeProperties = "");

        TEntity GetByID(object id);

        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity);

        void Update(TEntity entity);

        void DeleteByID(object id);

        void Delete(TEntity entityToDelete);
    }
}
