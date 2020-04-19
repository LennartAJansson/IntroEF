using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using WorkloadsDb.Abstract;
using WorkloadsDb.Model;

namespace WorkloadsDb
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        private readonly IWorkloadContext context;
        private bool disposed = false;

        public UnitOfWork(IWorkloadContext context) => this.context = context;

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IPOCOClass
        {
            if (repositories.Keys.Contains(typeof(TEntity)) == true)
            {
                return repositories[typeof(TEntity)] as IGenericRepository<TEntity>;
            }

            IGenericRepository<TEntity> repo = new GenericRepository<TEntity>(context);

            repositories.Add(typeof(TEntity), repo);

            return repo;
        }

        public int Save() => context.SaveChanges();

        public async Task<int> SaveAsync() => await context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
