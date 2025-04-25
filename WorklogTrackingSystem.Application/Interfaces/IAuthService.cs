using WorklogTrackingSystem.Domain.DTOs;

namespace WorklogTrackingSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(UserLoginDto userLogin);

        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequest);
    }
}