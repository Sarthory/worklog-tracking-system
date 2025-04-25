using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Domain.DTOs;

namespace WorklogTrackingSystem.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> CreateTokenResponse(User user);
        
        Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken);
    }
} 