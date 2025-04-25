using Microsoft.EntityFrameworkCore;
using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Infrastructure.Interfaces;
using WorklogTrackingSystem.Infrastructure.Configuration;

namespace WorklogTrackingSystem.Infrastructure.Data
{
    public class SqliteDbContext : DbContext, IDbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Worklog> Worklogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new WorklogConfiguration());

        }
    }
}