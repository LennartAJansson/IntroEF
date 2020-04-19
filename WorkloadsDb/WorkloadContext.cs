
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WorkloadsDb.Abstract;
using WorkloadsDb.Model;

namespace WorkloadsDb
{
    public class WorkloadContext : DbContext, IWorkloadContext
    {
        //You could either inject a LoggerFactory (will use Serilog and everything else that is configured by default)
        private readonly ILoggerFactory loggerFactory;

        //Or you could create your own LoggerFactory
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
            builder
                .AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                .AddConsole());

        public DbSet<Person> People { get; set; }
        public DbSet<Workload> Workloads { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        public WorkloadContext(DbContextOptions<WorkloadContext> options, ILoggerFactory loggerFactory)
            : base(options) => this.loggerFactory = loggerFactory;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WorkloadsIntro;Trusted_Connection=True;MultipleActiveResultSets=true");
            //}

            //Assign the LoggerFactory for the DbContext
            //optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Person>().ToTable("Persons")
            //modelBuilder.Entity<Person>().HasMany(s => s.Workloads).WithOne(s => s.Person);
            //modelBuilder.Entity<Person>().Property("City").HasMaxLength(20);
        }
    }
}
