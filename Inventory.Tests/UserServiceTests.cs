using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.UserDto;
using Inventory.Application.Services.UserService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<UserRequest>> _validatorMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly UserService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<UserRequest>>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _service = new UserService(
            _repositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _passwordHasherMock.Object);
    }

    private static User CreateUser(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test User",
        UserName = "testuser",
        Password = "hashed_password",
        Email = "test@example.com",
        RoleId = 1,
        CreatedAt = DateTime.UtcNow
    };

    private static UserRequest CreateRequest() => new()
    {
        Name = "Test User",
        UserName = "testuser",
        Password = "plaintext_password",
        Email = "test@example.com",
        RoleId = 1
    };

    [Fact]
    public async Task GetUsersAsync_ReturnsPagedResults()
    {
        var user = CreateUser();
        var response = new UserResponse { Id = user.Id, Name = user.Name };
        var paginatedList = new PaginatedList<User>([user], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetUsersAsync(_businessId, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<UserResponse>>(It.IsAny<List<User>>()))
            .Returns([response]);

        var result = await _service.GetUsersAsync(new UserSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(user.Name, result.Items[0].Name);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUser_WhenExists()
    {
        var user = CreateUser();
        var response = new UserResponse { Id = user.Id, Name = user.Name };

        _repositoryMock.Setup(r => r.GetUserByIdAsync(user.Id, _businessId))
            .ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user))
            .Returns(response);

        var result = await _service.GetUserByIdAsync(user.Id, _businessId);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var userId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetUserByIdAsync(userId, _businessId))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetUserByIdAsync(userId, _businessId));
    }

    [Fact]
    public async Task CreateUserAsync_HashesPasswordAndCreatesUser()
    {
        var request = CreateRequest();
        var user = CreateUser();
        var response = new UserResponse { Id = user.Id, Name = user.Name };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<User>(request))
            .Returns(user);
        _passwordHasherMock.Setup(h => h.Hash(request.Password))
            .Returns("hashed_password");
        _repositoryMock.Setup(r => r.CreateUserAsync(user))
            .ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user))
            .Returns(response);

        var result = await _service.CreateUserAsync(request, _businessId);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        _passwordHasherMock.Verify(h => h.Hash(request.Password), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_UpdatesUser_WhenExists()
    {
        var userId = Guid.NewGuid();
        var request = CreateRequest();
        var user = CreateUser(userId);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetUserByIdAsync(userId, _businessId))
            .ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map(request, user));

        await _service.UpdateUserAsync(userId, request, _businessId);

        _repositoryMock.Verify(r => r.UpdateUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var userId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetUserByIdAsync(userId, _businessId))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateUserAsync(userId, CreateRequest(), _businessId));
    }

    [Fact]
    public async Task DeleteUserAsync_DeletesUser_WhenExists()
    {
        var user = CreateUser();

        _repositoryMock.Setup(r => r.GetUserByIdAsync(user.Id, _businessId))
            .ReturnsAsync(user);

        await _service.DeleteUserAsync(user.Id, _businessId);

        _repositoryMock.Verify(r => r.DeleteUserAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var userId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetUserByIdAsync(userId, _businessId))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteUserAsync(userId, _businessId));
    }
}
