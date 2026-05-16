using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseProductDto;
using Inventory.Application.Services.WarehouseProductService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class WarehouseProductServiceTests
{
    private readonly Mock<IWarehouseRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WarehouseProductService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public WarehouseProductServiceTests()
    {
        _repositoryMock = new Mock<IWarehouseRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new WarehouseProductService(_repositoryMock.Object, _mapperMock.Object);
    }

    private static Warehouse CreateWarehouse(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Warehouse",
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task GetProductsByWarehousesAsync_ReturnsPagedProducts_WhenWarehouseExists()
    {
        var warehouse = CreateWarehouse();
        var warehouseProduct = new WarehouseProduct { WarehouseId = warehouse.Id, ProductId = 1, Stock = 10 };
        var response = new WarehouseProductResponse { Id = 1, Name = "Product A" };
        var paginatedList = new PaginatedList<WarehouseProduct>([warehouseProduct], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouse.Id, _businessId)).ReturnsAsync(warehouse);
        _repositoryMock.Setup(r => r.GetProductsByWarehousesAsync(warehouse.Id, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<WarehouseProductResponse>>(It.IsAny<List<WarehouseProduct>>()))
            .Returns([response]);

        var result = await _service.GetProductsByWarehousesAsync(warehouse.Id, new ProductSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task GetProductsByWarehousesAsync_ThrowsKeyNotFoundException_WhenWarehouseNotExists()
    {
        var warehouseId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouseId, _businessId))
            .ReturnsAsync((Warehouse?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetProductsByWarehousesAsync(warehouseId, new ProductSearchParams(), _businessId));
    }

    [Fact]
    public async Task AddProductsToWarehouseAsync_AddsProducts_WhenWarehouseExists()
    {
        var warehouse = CreateWarehouse();
        var requests = new List<WarehouseProductRequest>
        {
            new() { ProductId = 1, Stock = 50, LowStock = 5 }
        };

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouse.Id, _businessId)).ReturnsAsync(warehouse);

        await _service.AddProductsToWarehouseAsync(warehouse.Id, requests, _businessId);

        _repositoryMock.Verify(r => r.AddProductsToWarehouseAsync(It.IsAny<List<WarehouseProduct>>()), Times.Once);
    }

    [Fact]
    public async Task AddProductsToWarehouseAsync_ThrowsKeyNotFoundException_WhenWarehouseNotExists()
    {
        var warehouseId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWarehouseByIdAsync(warehouseId, _businessId))
            .ReturnsAsync((Warehouse?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.AddProductsToWarehouseAsync(warehouseId, [], _businessId));
    }
}
