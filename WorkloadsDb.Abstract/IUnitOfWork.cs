using System;
using System.Threading.Tasks;
using WorkloadsDb.Model;

namespace WorkloadsDb.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IPOCOClass;

        int Save();
        Task<int> SaveAsync();
    }
}
