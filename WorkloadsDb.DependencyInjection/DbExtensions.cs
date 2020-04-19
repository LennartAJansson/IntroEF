using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using WorkloadsDb;
using WorkloadsDb.Abstract;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DbExtensions
    {
        public static readonly string defaultConnectionString = "Server=(localdb)\\mssqllocaldb;Database=WorkloadsWpf;Trusted_Connection=True;MultipleActiveResultSets=true";

        //This extension is for IServiceCollection so you could add db support on this level
        public static IServiceCollection AddWorkloadDb(this IServiceCollection serviceCollection, Func<IConfiguration, string> GetConnectionString = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            //Build a temporary service provider to be able to get services from the IOC 
            var sp = serviceCollection.BuildServiceProvider();
            var configuration = sp.GetRequiredService<IConfiguration>();

            string connectionString = GetConnectionString != null ?
                GetConnectionString(configuration) :
                defaultConnectionString;

            serviceCollection.AddDbContext<IWorkloadContext, WorkloadContext>(dbContextOptionsBuilder =>
                dbContextOptionsBuilder.UseSqlServer(connectionString), serviceLifetime, serviceLifetime);

            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddTransient<IWorkloadService, WorkloadService>();

            return serviceCollection;
        }
    }
}
