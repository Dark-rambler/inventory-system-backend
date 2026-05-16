using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.Services.BranchProductService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class BranchProductServiceTests
{
    private readonly Mock<IBranchRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly BranchProductService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public BranchProductServiceTests()
    {
        _repositoryMock = new Mock<IBranchRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new BranchProductService(_repositoryMock.Object, _mapperMock.Object);
    }

    private static Branch CreateBranch(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Branch",
        Telephone = "5551234567",
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task GetProductsByBranchAsync_ReturnsPagedProducts_WhenBranchExists()
    {
        var branch = CreateBranch();
        var branchProduct = new BranchProduct { BranchId = branch.Id, ProductId = 1, Stock = 10 };
        var response = new BranchProductResponse { Id = 1, Name = "Product A" };
        var paginatedList = new PaginatedList<BranchProduct>([branchProduct], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branch.Id, _businessId)).ReturnsAsync(branch);
        _repositoryMock.Setup(r => r.GetProductsByBranchAsync(branch.Id, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<BranchProductResponse>>(It.IsAny<List<BranchProduct>>()))
            .Returns([response]);

        var result = await _service.GetProductsByBranchAsync(branch.Id, new ProductSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task GetProductsByBranchAsync_ThrowsKeyNotFoundException_WhenBranchNotExists()
    {
        var branchId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId))
            .ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetProductsByBranchAsync(branchId, new ProductSearchParams(), _businessId));
    }

    [Fact]
    public async Task AddProductsToBranchAsync_AddsProducts_WhenBranchExists()
    {
        var branch = CreateBranch();
        var requests = new List<BranchProductRequest>
        {
            new() { ProductId = 1, Price = 9.99, Stock = 50, LowStock = 5 }
        };

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branch.Id, _businessId)).ReturnsAsync(branch);

        await _service.AddProductsToBranchAsync(branch.Id, requests, _businessId);

        _repositoryMock.Verify(r => r.AddProductsToBranchAsync(It.IsAny<IEnumerable<BranchProduct>>()), Times.Once);
    }

    [Fact]
    public async Task AddProductsToBranchAsync_ThrowsKeyNotFoundException_WhenBranchNotExists()
    {
        var branchId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId))
            .ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.AddProductsToBranchAsync(branchId, [], _businessId));
    }

    [Fact]
    public async Task DeleteProductsAsync_DeletesProducts_WhenBranchExists()
    {
        var branch = CreateBranch();
        var productIds = new List<int> { 1, 2 };
        var branchProducts = new List<BranchProduct>
        {
            new() { BranchId = branch.Id, ProductId = 1 },
            new() { BranchId = branch.Id, ProductId = 2 }
        };

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branch.Id, _businessId)).ReturnsAsync(branch);
        _repositoryMock.Setup(r => r.GetBranchProductsByProductIdsAsync(branch.Id, productIds))
            .ReturnsAsync(branchProducts);

        await _service.DeleteProductsAsync(branch.Id, productIds, _businessId);

        _repositoryMock.Verify(r => r.DeleteProductsAsync(It.IsAny<IEnumerable<BranchProduct>>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProductsAsync_ThrowsKeyNotFoundException_WhenBranchNotExists()
    {
        var branchId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId))
            .ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteProductsAsync(branchId, [], _businessId));
    }
}
