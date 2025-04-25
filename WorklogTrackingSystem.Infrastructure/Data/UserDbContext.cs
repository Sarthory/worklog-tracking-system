using Microsoft.EntityFrameworkCore;
using WorklogTrackingSystem.Domain.Entities;

namespace WorklogTrackingSystem.Infrastructure.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options, DatabaseSettings databaseSettings) : DbContext(options)
{
    private readonly DatabaseSettings _databaseSettings = databaseSettings;

    public DbSet<User> Users { get; set; }
    public DbSet<Worklog> Worklogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (_databaseSettings.Provider?.ToLower() == "sqlite")
            {
                optionsBuilder.UseSqlite(_databaseSettings.ConnectionString, b =>
                                b.MigrationsAssembly("WorklogTrackingSystem.Infrastructure.Migrations.SQLite"));
            }
            else
            {
                optionsBuilder.UseSqlServer(_databaseSettings.ConnectionString, b =>
                                b.MigrationsAssembly("WorklogTrackingSystem.Infrastructure.Migrations.SqlServer"));
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.Login).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.DailyMinHours).IsRequired();
            entity.Property(e => e.DailyMaxHours).IsRequired();
            entity.Property(e => e.RefreshToken).IsRequired(false);
            entity.Property(e => e.RefreshTokenExpiryTime).IsRequired(false);
        });

        modelBuilder.Entity<Worklog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.WorkedHours).IsRequired();
            entity.Property(e => e.TaskId).IsRequired(false);
            entity.Property(e => e.Description).IsRequired(false);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
