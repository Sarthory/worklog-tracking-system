using Microsoft.EntityFrameworkCore;
using WorklogTrackingSystem.Domain.Entities;

namespace WorklogTrackingSystem.Infrastructure.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbSet<User> Users { get; set; }
        DbSet<Worklog> Worklogs { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}