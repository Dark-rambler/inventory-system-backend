using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.Services.BranchService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class BranchServiceTests
{
    private readonly Mock<IBranchRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<BranchRequest>> _validatorMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IBusinessSaleCounterRepository> _saleCounterRepositoryMock;
    private readonly BranchService _service;
    private readonly Guid _businessId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

    public BranchServiceTests()
    {
        _repositoryMock = new Mock<IBranchRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<BranchRequest>>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _saleCounterRepositoryMock = new Mock<IBusinessSaleCounterRepository>();
        _service = new BranchService(
            _repositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _currentUserServiceMock.Object,
            _dateTimeProviderMock.Object,
            _saleCounterRepositoryMock.Object);
    }

    private static Branch CreateBranch(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Branch",
        Telephone = "5551234567",
        CreatedAt = DateTime.UtcNow
    };

    private static BranchRequest CreateRequest() => new()
    {
        Name = "Test Branch",
        Telephone = "5551234567",
        Location = new()
    };

    [Fact]
    public async Task GetBranchesAsync_ReturnsPagedResults()
    {
        var branch = CreateBranch();
        var response = new BranchResponse { Id = branch.Id, Name = branch.Name };
        var paginatedList = new PaginatedList<Branch>([branch], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetBranchesAsync(_businessId, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<BranchResponse>>(It.IsAny<List<Branch>>()))
            .Returns([response]);

        var result = await _service.GetBranchesAsync(new BranchSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(branch.Name, result.Items[0].Name);
    }

    [Fact]
    public async Task GetBranchByIdAsync_ReturnsBranch_WhenExists()
    {
        var branch = CreateBranch();
        var response = new BranchResponse { Id = branch.Id, Name = branch.Name };

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branch.Id, _businessId)).ReturnsAsync(branch);
        _mapperMock.Setup(m => m.Map<BranchResponse>(branch)).Returns(response);

        var result = await _service.GetBranchByIdAsync(branch.Id, _businessId);

        Assert.NotNull(result);
        Assert.Equal(branch.Id, result.Id);
    }

    [Fact]
    public async Task GetBranchByIdAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var branchId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId))
            .ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetBranchByIdAsync(branchId, _businessId));
    }

    [Fact]
    public async Task CreateBranchAsync_CreatesAndReturnsBranch()
    {
        var request = CreateRequest();
        var branch = CreateBranch();
        var response = new BranchResponse { Id = branch.Id, Name = branch.Name };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<BranchRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<Branch>(request)).Returns(branch);
        _repositoryMock.Setup(r => r.CreateBranchAsync(branch)).ReturnsAsync(branch);
        _mapperMock.Setup(m => m.Map<BranchResponse>(branch)).Returns(response);

        var result = await _service.CreateBranchAsync(request, _businessId);

        Assert.NotNull(result);
        Assert.Equal(branch.Id, result.Id);
    }

    [Fact]
    public async Task UpdateBranchAsync_UpdatesBranch_WhenExists()
    {
        var branchId = Guid.NewGuid();
        var request = CreateRequest();
        var branch = CreateBranch(branchId);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<BranchRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId)).ReturnsAsync(branch);
        _mapperMock.Setup(m => m.Map(request, branch));

        await _service.UpdateBranchAsync(branchId, request, _businessId);

        _repositoryMock.Verify(r => r.UpdateBranchAsync(It.IsAny<Branch>()), Times.Once);
    }

    [Fact]
    public async Task UpdateBranchAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var branchId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId))
            .ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateBranchAsync(branchId, CreateRequest(), _businessId));
    }

    [Fact]
    public async Task DeleteBranchAsync_DeletesBranch_WhenExists()
    {
        var branch = CreateBranch();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branch.Id, _businessId)).ReturnsAsync(branch);

        await _service.DeleteBranchAsync(branch.Id, _businessId);

        _repositoryMock.Verify(r => r.DeleteBranchAsync(branch), Times.Once);
    }

    [Fact]
    public async Task DeleteBranchAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var branchId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId))
            .ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteBranchAsync(branchId, _businessId));
    }

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
    public async Task CreateSaleAsync_CreatesSale_WhenBranchExistsAndStockIsSufficient()
    {
        var branch = CreateBranch();
        var productId = 1;
        var branchProduct = new BranchProduct
        {
            BranchId = branch.Id,
            ProductId = productId,
            Stock = 100,
            Price = 9.99
        };
        var request = new SaleRequest
        {
            CustomerId = Guid.NewGuid(),
            SaleDetails = [new SaleDetailRequest { ProductId = productId, Quantity = 5 }]
        };

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branch.Id, _businessId)).ReturnsAsync(branch);
        _currentUserServiceMock.Setup(u => u.GetCurrentUserId()).Returns(_userId);
        _dateTimeProviderMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);
        _repositoryMock.Setup(r => r.GetBranchProductsByProductIdsAsync(branch.Id, It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync([branchProduct]);
        _saleCounterRepositoryMock.Setup(c => c.GetNextFolioAsync(_businessId)).ReturnsAsync("POS-0001");

        await _service.CreateSaleAsync(branch.Id, request, _businessId);

        _repositoryMock.Verify(r => r.CreateSaleAsync(
            It.IsAny<Sale>(),
            It.IsAny<List<InventoryMovement>>(),
            It.IsAny<List<BranchProduct>>(),
            It.IsAny<AuditHistory>()), Times.Once);
    }

    [Fact]
    public async Task CreateSaleAsync_ThrowsKeyNotFoundException_WhenBranchNotExists()
    {
        var branchId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId))
            .ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.CreateSaleAsync(branchId, new SaleRequest(), _businessId));
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
    public async Task GetSalesByBranchAsync_ReturnsSales_WhenBranchExists()
    {
        var branch = CreateBranch();
        var sale = new Sale { Id = Guid.NewGuid(), Total = 50.0 };
        var response = new SaleResponse { Id = sale.Id, Total = sale.Total };
        var paginatedList = new PaginatedList<Sale>([sale], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branch.Id, _businessId)).ReturnsAsync(branch);
        _repositoryMock.Setup(r => r.GetSalesByBranchAsync(_businessId, branch.Id, null, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<SaleResponse>>(It.IsAny<List<Sale>>()))
            .Returns([response]);

        var result = await _service.GetSalesByBranchAsync(branch.Id, new SaleSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(sale.Id, result.Items[0].Id);
    }

    [Fact]
    public async Task GetSalesByBranchAsync_ThrowsKeyNotFoundException_WhenBranchNotExists()
    {
        var branchId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetBranchByIdAsync(branchId, _businessId))
            .ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetSalesByBranchAsync(branchId, new SaleSearchParams(), _businessId));
    }
}
