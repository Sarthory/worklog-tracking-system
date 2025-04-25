using WorklogTrackingSystem.Domain.DTOs;
using WorklogTrackingSystem.Domain.Entities;

namespace WorklogTrackingSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(UserDto user);

        Task<User?> UpdateAsync(int id, UserDto user);

        Task<User?> GetByIdAsync(int id);

        Task<PagedResultDto<UserDto>> GetPagedAsync(int page, int pageSize);
    }
}