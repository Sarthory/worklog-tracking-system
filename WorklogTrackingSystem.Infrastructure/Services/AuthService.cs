using WorklogTrackingSystem.Application.Interfaces;
using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Domain.DTOs;
using WorklogTrackingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WorklogTrackingSystem.Infrastructure.Services
{
    public class AuthService(UserDbContext context, IConfiguration configuration) : IAuthService
    {
        private readonly UserDbContext _context = context;
        private readonly TokenService _tokenService = new(context, configuration);

        public async Task<TokenResponseDto?> LoginAsync(UserLoginDto userLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userLogin.Login);

            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userLogin.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await _tokenService.CreateTokenResponse(user);
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequest)
        {
            var user = await _tokenService.ValidateRefreshTokenAsync(refreshTokenRequest.UserId, refreshTokenRequest.RefreshToken);

            if (user is null) return null;

            return await _tokenService.CreateTokenResponse(user);
        }
    }
}