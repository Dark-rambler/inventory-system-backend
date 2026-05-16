using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.Services.SaleService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class SaleServiceTests
{
    private readonly Mock<IBranchRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IBusinessSaleCounterRepository> _saleCounterRepositoryMock;
    private readonly SaleService _service;
    private readonly Guid _businessId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

    public SaleServiceTests()
    {
        _repositoryMock = new Mock<IBranchRepository>();
        _mapperMock = new Mock<IMapper>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _saleCounterRepositoryMock = new Mock<IBusinessSaleCounterRepository>();
        _service = new SaleService(
            _repositoryMock.Object,
            _mapperMock.Object,
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
