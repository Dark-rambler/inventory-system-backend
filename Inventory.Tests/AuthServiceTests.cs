using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Abstracts.Clients;
using Inventory.Application.DataTransferObjects.AuthDto;
using Inventory.Application.Services.AuthService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);
        _service = new AuthService(
            _repositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _jwtServiceMock.Object,
            _passwordHasherMock.Object,
            _dateTimeProviderMock.Object
        );
    }

    private static User CreateUser() => new()
    {
        Id = Guid.NewGuid(),
        Name = "Test User",
        UserName = "testuser",
        Password = "hashed_password",
        Email = "test@example.com",
        RoleId = 1
    };

    private static RefreshToken CreateRefreshToken(Guid userId, bool isRevoked = false, DateTime? expiresAt = null) => new()
    {
        Id = Guid.NewGuid(),
        Token = "valid_refresh_token",
        UserId = userId,
        User = CreateUser(),
        IsRevoked = isRevoked,
        ExpiresAt = expiresAt ?? DateTime.UtcNow.AddDays(7),
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task LoginAsync_ReturnsTokens_WhenCredentialsAreValid()
    {
        var user = CreateUser();
        var request = new LoginRequest("testuser", "plain_password");

        _repositoryMock.Setup(r => r.GetUserByUserNameAsync(request.UserName))
            .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.Verify(request.Password, user.Password))
            .Returns(true);
        _jwtServiceMock.Setup(j => j.GenerateJwtToken(user))
            .Returns("jwt_token");
        _jwtServiceMock.Setup(j => j.GenerateRefreshToken())
            .Returns("refresh_token");
        _refreshTokenRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.LoginAsync(request);

        Assert.NotNull(result);
        Assert.Equal("jwt_token", result.Token);
        Assert.Equal("refresh_token", result.RefreshToken);
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedAccessException_WhenUserNotFound()
    {
        var request = new LoginRequest("unknown", "password");

        _repositoryMock.Setup(r => r.GetUserByUserNameAsync(request.UserName))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedAccessException_WhenPasswordIsInvalid()
    {
        var user = CreateUser();
        var request = new LoginRequest("testuser", "wrong_password");

        _repositoryMock.Setup(r => r.GetUserByUserNameAsync(request.UserName))
            .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.Verify(request.Password, user.Password))
            .Returns(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.LoginAsync(request));
    }

    [Fact]
    public async Task RefreshTokenAsync_ReturnsNewTokens_WhenRefreshTokenIsValid()
    {
        var user = CreateUser();
        var storedToken = CreateRefreshToken(user.Id);
        var request = new RefreshTokenRequest("valid_refresh_token");

        _refreshTokenRepositoryMock.Setup(r => r.GetByTokenAsync(request.RefreshToken))
            .ReturnsAsync(storedToken);
        _refreshTokenRepositoryMock.Setup(r => r.RevokeAsync(storedToken))
            .Returns(Task.CompletedTask);
        _jwtServiceMock.Setup(j => j.GenerateJwtToken(storedToken.User))
            .Returns("new_jwt_token");
        _jwtServiceMock.Setup(j => j.GenerateRefreshToken())
            .Returns("new_refresh_token");
        _refreshTokenRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.RefreshTokenAsync(request);

        Assert.NotNull(result);
        Assert.Equal("new_jwt_token", result.Token);
        Assert.Equal("new_refresh_token", result.RefreshToken);
        _refreshTokenRepositoryMock.Verify(r => r.RevokeAsync(storedToken), Times.Once);
    }

    [Fact]
    public async Task RefreshTokenAsync_ThrowsUnauthorizedAccessException_WhenTokenNotFound()
    {
        var request = new RefreshTokenRequest("nonexistent_token");

        _refreshTokenRepositoryMock.Setup(r => r.GetByTokenAsync(request.RefreshToken))
            .ReturnsAsync((RefreshToken?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.RefreshTokenAsync(request));
    }

    [Fact]
    public async Task RefreshTokenAsync_ThrowsUnauthorizedAccessException_WhenTokenIsRevoked()
    {
        var user = CreateUser();
        var revokedToken = CreateRefreshToken(user.Id, isRevoked: true);
        var request = new RefreshTokenRequest("revoked_token");

        _refreshTokenRepositoryMock.Setup(r => r.GetByTokenAsync(request.RefreshToken))
            .ReturnsAsync(revokedToken);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.RefreshTokenAsync(request));
    }

    [Fact]
    public async Task RefreshTokenAsync_ThrowsUnauthorizedAccessException_WhenTokenIsExpired()
    {
        var user = CreateUser();
        var expiredToken = CreateRefreshToken(user.Id, expiresAt: DateTime.UtcNow.AddDays(-1));
        var request = new RefreshTokenRequest("expired_token");

        _refreshTokenRepositoryMock.Setup(r => r.GetByTokenAsync(request.RefreshToken))
            .ReturnsAsync(expiredToken);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.RefreshTokenAsync(request));
    }
}
