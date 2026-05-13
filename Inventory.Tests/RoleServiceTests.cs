using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.RoleDto;
using Inventory.Application.Services.RoleService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class RoleServiceTests
{
    private readonly Mock<IRoleRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RoleService _service;

    public RoleServiceTests()
    {
        _repositoryMock = new Mock<IRoleRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new RoleService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetRolesAsync_ReturnsAllRoles()
    {
        var roles = new List<Role>
        {
            new() { Id = 1, Name = "Admin", Description = "Administrator" },
            new() { Id = 2, Name = "Employee", Description = "Regular employee" }
        };
        var responses = roles.Select(r => new RoleResponse(r.Id, r.Name, r.Description)).ToList();

        _repositoryMock.Setup(r => r.GetRolesAsync())
            .ReturnsAsync(roles);
        _mapperMock.Setup(m => m.Map<List<RoleResponse>>(roles))
            .Returns(responses);

        var result = await _service.GetRolesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Admin", result[0].Name);
        Assert.Equal("Employee", result[1].Name);
    }

    [Fact]
    public async Task GetRolesAsync_ReturnsEmptyList_WhenNoRoles()
    {
        _repositoryMock.Setup(r => r.GetRolesAsync())
            .ReturnsAsync([]);
        _mapperMock.Setup(m => m.Map<List<RoleResponse>>(It.IsAny<List<Role>>()))
            .Returns([]);

        var result = await _service.GetRolesAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
