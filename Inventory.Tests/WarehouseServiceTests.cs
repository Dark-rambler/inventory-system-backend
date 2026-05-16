using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Application.Services.WarehouseService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class WarehouseServiceTests
{
    private readonly Mock<IWarehouseRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WarehouseService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public WarehouseServiceTests()
    {
        _repositoryMock = new Mock<IWarehouseRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new WarehouseService(_repositoryMock.Object, _mapperMock.Object);
    }

    private static Warehouse CreateWarehouse(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Warehouse",
        CreatedAt = DateTime.UtcNow
    };

    private static WarehouseRequest CreateRequest() => new()
    {
        Name = "Test Warehouse",
        Location = new()
    };

    [Fact]
    public async Task GetWarehousesAsync_ReturnsPagedResults()
    {
        var warehouse = CreateWarehouse();
        var response = new WarehouseResponse { Id = warehouse.Id, Name = warehouse.Name };
        var paginatedList = new PaginatedList<Warehouse>([warehouse], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetWarehousesAsync(_businessId, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<WarehouseResponse>>(It.IsAny<List<Warehouse>>()))
            .Returns([response]);

        var result = await _service.GetWarehousesAsync(new WarehouseSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(warehouse.Name, result.Items[0].Name);
    }

    [Fact]
    public async Task GetWarehouseByIdAsync_ReturnsWarehouse_WhenExists()
    {
        var warehouse = CreateWarehouse();
        var response = new WarehouseResponse { Id = warehouse.Id, Name = warehouse.Name };

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouse.Id, _businessId))
            .ReturnsAsync(warehouse);
        _mapperMock.Setup(m => m.Map<WarehouseResponse>(warehouse))
            .Returns(response);

        var result = await _service.GetWarehouseByIdAsync(warehouse.Id, _businessId);

        Assert.NotNull(result);
        Assert.Equal(warehouse.Id, result.Id);
    }

    [Fact]
    public async Task GetWarehouseByIdAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var warehouseId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouseId, _businessId))
            .ReturnsAsync((Warehouse?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetWarehouseByIdAsync(warehouseId, _businessId));
    }

    [Fact]
    public async Task CreateWarehouseAsync_CreatesAndReturnsWarehouse()
    {
        var request = CreateRequest();
        var warehouse = CreateWarehouse();
        var response = new WarehouseResponse { Id = warehouse.Id, Name = warehouse.Name };

        _mapperMock.Setup(m => m.Map<Warehouse>(request)).Returns(warehouse);
        _repositoryMock.Setup(r => r.CreateWarehouseAsync(warehouse)).ReturnsAsync(warehouse);
        _mapperMock.Setup(m => m.Map<WarehouseResponse>(warehouse)).Returns(response);

        var result = await _service.CreateWarehouseAsync(request, _businessId);

        Assert.NotNull(result);
        Assert.Equal(warehouse.Id, result.Id);
    }

    [Fact]
    public async Task UpdateWarehouseAsync_UpdatesWarehouse_WhenExists()
    {
        var warehouseId = Guid.NewGuid();
        var request = CreateRequest();
        var warehouse = CreateWarehouse(warehouseId);

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouseId, _businessId)).ReturnsAsync(warehouse);
        _mapperMock.Setup(m => m.Map(request, warehouse));

        await _service.UpdateWarehouseAsync(warehouseId, request, _businessId);

        _repositoryMock.Verify(r => r.UpdateWarehouseAsync(It.IsAny<Warehouse>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWarehouseAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var warehouseId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouseId, _businessId))
            .ReturnsAsync((Warehouse?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateWarehouseAsync(warehouseId, CreateRequest(), _businessId));
    }

    [Fact]
    public async Task DeleteWarehouseAsync_DeletesWarehouse_WhenExists()
    {
        var warehouse = CreateWarehouse();

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouse.Id, _businessId)).ReturnsAsync(warehouse);

        await _service.DeleteWarehouseAsync(warehouse.Id, _businessId);

        _repositoryMock.Verify(r => r.DeleteWarehouseAsync(warehouse), Times.Once);
    }

    [Fact]
    public async Task DeleteWarehouseAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var warehouseId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouseId, _businessId))
            .ReturnsAsync((Warehouse?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteWarehouseAsync(warehouseId, _businessId));
    }
}
