using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.PurchaseDto;
using Inventory.Application.Services.PurchaseService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class PurchaseServiceTests
{
    private readonly Mock<IPurchaseRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<PurchaseRequest>> _validatorMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly PurchaseService _service;
    private readonly Guid _businessId = Guid.NewGuid();
    private readonly Guid _userId = Guid.NewGuid();

    public PurchaseServiceTests()
    {
        _repositoryMock = new Mock<IPurchaseRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<PurchaseRequest>>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _service = new PurchaseService(
            _repositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _currentUserServiceMock.Object,
            _dateTimeProviderMock.Object);
    }

    private void SetupCommonMocks()
    {
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<PurchaseRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _currentUserServiceMock.Setup(u => u.GetCurrentUserId()).Returns(_userId);
        _dateTimeProviderMock.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);
    }

    [Fact]
    public async Task GetPurchasesAsync_ReturnsPagedResults()
    {
        var purchase = new Purchase { Id = Guid.NewGuid(), Total = 100.0 };
        var response = new PurchaseResponse { Id = purchase.Id, Total = purchase.Total };
        var paginatedList = new PaginatedList<Purchase>([purchase], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetPurchasesAsync(_businessId, null, null, null, null, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<PurchaseResponse>>(It.IsAny<List<Purchase>>()))
            .Returns([response]);

        var result = await _service.GetPurchasesAsync(new PurchaseSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(purchase.Id, result.Items[0].Id);
    }

    [Fact]
    public async Task CreatePurchaseAsync_CreatesPurchase_WhenDestinationIsBranch()
    {
        var branchId = Guid.NewGuid();
        var productId = 1;
        var branchProduct = new BranchProduct { BranchId = branchId, ProductId = productId, Stock = 0 };
        var request = new PurchaseRequest
        {
            ProviderId = Guid.NewGuid(),
            BranchId = branchId,
            PurchaseDetails = [new PurchaseDetailRequest { ProductId = productId, Quantity = 10, Price = 5.0 }]
        };

        SetupCommonMocks();
        _repositoryMock.Setup(r => r.GetBranchProductsByProductIdsAsync(branchId, It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync([branchProduct]);

        await _service.CreatePurchaseAsync(request, _businessId);

        _repositoryMock.Verify(r => r.CreatePurchaseAsync(
            It.IsAny<Purchase>(),
            It.IsAny<List<InventoryMovement>>(),
            It.Is<List<BranchProduct>?>(l => l != null),
            It.Is<List<WarehouseProduct>?>(l => l == null),
            It.IsAny<AuditHistory>()), Times.Once);
    }

    [Fact]
    public async Task CreatePurchaseAsync_CreatesPurchase_WhenDestinationIsWarehouse()
    {
        var warehouseId = Guid.NewGuid();
        var productId = 2;
        var warehouseProduct = new WarehouseProduct { WarehouseId = warehouseId, ProductId = productId, Stock = 0 };
        var request = new PurchaseRequest
        {
            ProviderId = Guid.NewGuid(),
            WarehouseId = warehouseId,
            PurchaseDetails = [new PurchaseDetailRequest { ProductId = productId, Quantity = 20, Price = 3.5 }]
        };

        SetupCommonMocks();
        _repositoryMock.Setup(r => r.GetWarehouseProductsByProductIdsAsync(warehouseId, It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync([warehouseProduct]);

        await _service.CreatePurchaseAsync(request, _businessId);

        _repositoryMock.Verify(r => r.CreatePurchaseAsync(
            It.IsAny<Purchase>(),
            It.IsAny<List<InventoryMovement>>(),
            It.Is<List<BranchProduct>?>(l => l == null),
            It.Is<List<WarehouseProduct>?>(l => l != null),
            It.IsAny<AuditHistory>()), Times.Once);
    }
}
