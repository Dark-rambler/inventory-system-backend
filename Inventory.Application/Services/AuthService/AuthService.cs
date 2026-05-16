using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Abstracts.Clients;
using Inventory.Application.DataTransferObjects.AuthDto;
using Inventory.Domain.Entities.Builders;

namespace Inventory.Application.Services.AuthService
{
    public class AuthService(
        IUserRepository repository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IPasswordHasher passwordHasher,
        IDateTimeProvider dateTimeProvider
    ) : IAuthService
    {
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await repository.GetUserByUserNameAsync(request.UserName)
                ?? throw new UnauthorizedAccessException("Invalid credentials");

            if (!passwordHasher.Verify(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var accessToken = jwtService.GenerateJwtToken(user);
            var rawRefreshToken = jwtService.GenerateRefreshToken();

            var refreshToken = new RefreshTokenBuilder()
                .WithToken(rawRefreshToken)
                .WithUserId(user.Id)
                .WithExpiresAt(dateTimeProvider.UtcNow.AddDays(7))
                .WithCreatedAt(dateTimeProvider.UtcNow)
                .Build();

            await refreshTokenRepository.CreateAsync(refreshToken);

            return new LoginResponse(accessToken, rawRefreshToken);
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken)
                ?? throw new UnauthorizedAccessException("Invalid refresh token");

            if (refreshToken.IsRevoked)
                throw new UnauthorizedAccessException("Refresh token has been revoked");

            if (refreshToken.ExpiresAt < dateTimeProvider.UtcNow)
                throw new UnauthorizedAccessException("Refresh token has expired");

            await refreshTokenRepository.RevokeAsync(refreshToken);

            var user = refreshToken.User;
            var newAccessToken = jwtService.GenerateJwtToken(user);
            var newRawRefreshToken = jwtService.GenerateRefreshToken();

            var newRefreshToken = new RefreshTokenBuilder()
                .WithToken(newRawRefreshToken)
                .WithUserId(user.Id)
                .WithExpiresAt(dateTimeProvider.UtcNow.AddDays(7))
                .WithCreatedAt(dateTimeProvider.UtcNow)
                .Build();

            await refreshTokenRepository.CreateAsync(newRefreshToken);

            return new LoginResponse(newAccessToken, newRawRefreshToken);
        }
    }
}
