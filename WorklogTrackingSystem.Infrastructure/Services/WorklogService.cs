using Microsoft.EntityFrameworkCore;
using WorklogTrackingSystem.Application.Interfaces;
using WorklogTrackingSystem.Domain.DTOs;
using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Domain.Enums;
using WorklogTrackingSystem.Infrastructure.Interfaces;

namespace WorklogTrackingSystem.Infrastructure.Services
{
    public class WorklogService(IDbContext context) : IWorklogService
    {
        private readonly IDbContext _context = context;

        public async Task<Worklog?> InsertAsync(WorklogDto worklog)
        {
            var newWorklog = new Worklog
            {
                UserId = worklog.UserId,
                Date = worklog.Date,
                WorkedHours = worklog.WorkedHours,
                Description = worklog.Description,
                TaskId = worklog.TaskId
            };

            _context.Worklogs.Add(newWorklog);
            await _context.SaveChangesAsync();
            return newWorklog;
        }

        public async Task<Worklog?> UpdateAsync(int id, WorklogDto worklog)
        {
            var existingWorklog = await _context.Worklogs.FindAsync(id);

            if (existingWorklog == null)
            {
                return null;
            }

            existingWorklog.Date = worklog.Date;
            existingWorklog.WorkedHours = worklog.WorkedHours;
            existingWorklog.Description = worklog.Description;
            existingWorklog.TaskId = worklog.TaskId;

            await _context.SaveChangesAsync();

            return existingWorklog;
        }

        public async Task<PagedResultDto<WorklogSummaryDto>> GetWorklogSummaryByDateAsync(int userId, int page, int pageSize, string? filter = null)
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page and pageSize must be greater than 0.");
            }

            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new { u.DailyMinHours, u.DailyMaxHours })
                .FirstOrDefaultAsync() ?? throw new ArgumentException($"User with ID {userId} not found.");

            if (user.DailyMinHours <= 0 || user.DailyMaxHours <= 0)
            {
                throw new Exception("User's DailyMinHours or DailyMaxHours is not set correctly.");
            }

            var query = _context.Worklogs
                .AsNoTracking()
                .Where(w => w.UserId == userId)
                .GroupBy(w => w.Date)
                .Select(g => new WorklogSummaryDto
                {
                    Date = g.Key,
                    WorklogCount = g.Count(),
                    Entries = g.ToList(),
                    TotalWorkedHours = g.Sum(w => w.WorkedHours),
                    Situation = g.Sum(w => w.WorkedHours) < user.DailyMinHours
                        ? WorklogSituation.Undertime.ToString()
                        : g.Sum(w => w.WorkedHours) > user.DailyMaxHours
                            ? WorklogSituation.Overtime.ToString()
                            : WorklogSituation.Normal.ToString()
                });

            if (filter is not null) query = query.Where(i => i.Situation == filter);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderByDescending(s => s.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<WorklogSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }
}