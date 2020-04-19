
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkloadsDb
{
    //This is used by add-migration and update-database
    public class WorkloadContextFactory : IDesignTimeDbContextFactory<WorkloadContext>
    {
        public WorkloadContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorkloadContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WorkloadsIntro;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new WorkloadContext(optionsBuilder.Options, null);
        }
    }
}
