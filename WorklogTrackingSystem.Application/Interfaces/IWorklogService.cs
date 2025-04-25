using WorklogTrackingSystem.Domain.DTOs;
using WorklogTrackingSystem.Domain.Entities;

namespace WorklogTrackingSystem.Application.Interfaces
{
    public interface IWorklogService
    {
        Task<Worklog?> InsertAsync(WorklogDto worklog);

        Task<Worklog?> UpdateAsync(int id, WorklogDto worklog);

        Task<PagedResultDto<WorklogSummaryDto>> GetWorklogSummaryByDateAsync(int userId, int page, int pageSize, string? filter = null);
    }
}