using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WorklogTrackingSystem.Application.Interfaces;
using WorklogTrackingSystem.Domain.DTOs;
using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Domain.Enums;
using WorklogTrackingSystem.Infrastructure.Data;

namespace WorklogTrackingSystem.Infrastructure.Services
{
    public class UserService(UserDbContext context) : IUserService
    {
        private readonly UserDbContext _context = context;

        public Task<User?> GetByIdAsync(int id)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> RegisterAsync(UserDto userData)
        {
            if (await _context.Users.AnyAsync(u => u.Login == userData.Login))
            {
                return null;
            }

            var newUser = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, userData.Password);

            newUser.Login = userData.Login;
            newUser.FirstName = userData.FirstName;
            newUser.LastName = userData.LastName;
            newUser.PasswordHash = hashedPassword;
            newUser.Role = Enum.Parse<Role>($"{userData.Role}");
            newUser.DailyMinHours = userData.DailyMinHours;
            newUser.DailyMaxHours = userData.DailyMaxHours;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public Task<User?> UpdateAsync(int id, UserDto user)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                .ContinueWith(async task =>
                {
                    var existingUser = await task;
                    if (existingUser == null)
                    {
                        return null;
                    }

                    existingUser.Login = user.Login;
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.DailyMinHours = user.DailyMinHours;
                    existingUser.DailyMaxHours = user.DailyMaxHours;

                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();

                    return existingUser;
                }).Unwrap();
        }

        public async Task<PagedResultDto<UserDto>> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Users
                .AsNoTracking()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Login = u.Login,
                    DailyMinHours = u.DailyMinHours,
                    DailyMaxHours = u.DailyMaxHours,
                    Role = u.Role.ToString(),
                });

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<UserDto>
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