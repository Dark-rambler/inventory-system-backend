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
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _service = new AuthService(_repositoryMock.Object, _jwtServiceMock.Object, _passwordHasherMock.Object);
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

    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenCredentialsAreValid()
    {
        var user = CreateUser();
        var request = new LoginRequest("testuser", "plain_password");

        _repositoryMock.Setup(r => r.GetUserByUserNameAsync(request.UserName))
            .ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.Verify(request.Password, user.Password))
            .Returns(true);
        _jwtServiceMock.Setup(j => j.GenerateJwtToken(user))
            .Returns("jwt_token");

        var result = await _service.LoginAsync(request);

        Assert.NotNull(result);
        Assert.Equal("jwt_token", result.Token);
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
}
