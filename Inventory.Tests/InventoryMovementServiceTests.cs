using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Application.Services.InventoryMovementService;
using Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;
using Moq;

namespace Inventory.Tests;

public class InventoryMovementServiceTests
{
    private readonly Mock<IInventoryMovementRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IInventoryMovementStrategy> _strategyMock;
    private readonly MovementStrategyResolver _resolver;
    private readonly InventoryMovementService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public InventoryMovementServiceTests()
    {
        _repositoryMock = new Mock<IInventoryMovementRepository>();
        _mapperMock = new Mock<IMapper>();
        _strategyMock = new Mock<IInventoryMovementStrategy>();
        _strategyMock.Setup(s => s.Type).Returns(EnumMovementType.Entry);
        _resolver = new MovementStrategyResolver([_strategyMock.Object]);
        _service = new InventoryMovementService(_repositoryMock.Object, _resolver, _mapperMock.Object);
    }

    [Fact]
    public async Task GetInventoryMovementsAsync_ReturnsPagedResults()
    {
        var movement = new InventoryMovement { Id = Guid.NewGuid(), Quantity = 10, Type = EnumMovementType.Entry };
        var response = new InventoryMovementResponse { Id = movement.Id, Quantity = movement.Quantity };
        var paginatedList = new PaginatedList<InventoryMovement>([movement], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetInventoryMovementsAsync(_businessId, null, null, null, null, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<InventoryMovementResponse>>(It.IsAny<List<InventoryMovement>>()))
            .Returns([response]);

        var result = await _service.GetInventoryMovementsAsync(new InventoryMovementSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(movement.Id, result.Items[0].Id);
    }

    [Fact]
    public async Task CreateInventoryMovementAsync_DelegatesToMatchingStrategy()
    {
        var movement = new InventoryMovement { Id = Guid.NewGuid(), Type = EnumMovementType.Entry };
        var response = new InventoryMovementResponse { Id = movement.Id };
        var request = new InventoryMovementRequest { ProductId = 1, Quantity = 5, Type = EnumMovementType.Entry };

        _strategyMock.Setup(s => s.ExecuteAsync(request, _businessId)).ReturnsAsync(movement);
        _mapperMock.Setup(m => m.Map<InventoryMovementResponse>(movement)).Returns(response);

        var result = await _service.CreateInventoryMovementAsync(request, _businessId);

        Assert.NotNull(result);
        Assert.Equal(movement.Id, result.Id);
        _strategyMock.Verify(s => s.ExecuteAsync(request, _businessId), Times.Once);
    }

    [Fact]
    public async Task CreateInventoryMovementAsync_ThrowsInvalidOperationException_WhenNoStrategyFound()
    {
        var request = new InventoryMovementRequest { ProductId = 1, Quantity = 5, Type = EnumMovementType.Exit };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateInventoryMovementAsync(request, _businessId));
    }
}
