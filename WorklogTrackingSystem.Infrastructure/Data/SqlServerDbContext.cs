using Microsoft.EntityFrameworkCore;
using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Infrastructure.Interfaces;
using WorklogTrackingSystem.Infrastructure.Configuration;

namespace WorklogTrackingSystem.Infrastructure.Data
{
    public class SqlServerDbContext : DbContext, IDbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Worklog> Worklogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new WorklogConfiguration());

            modelBuilder.Entity<User>().Property(u => u.Id).UseIdentityColumn();
            modelBuilder.Entity<Worklog>().Property(w => w.Id).UseIdentityColumn();
        }
    }
}