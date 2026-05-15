using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.AuditHistoryDto;
using Inventory.Application.Services.AuditHistoryService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class AuditHistoryServiceTests
{
    private readonly Mock<IAuditHistoryRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuditHistoryService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public AuditHistoryServiceTests()
    {
        _repositoryMock = new Mock<IAuditHistoryRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new AuditHistoryService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAuditHistoriesAsync_ReturnsPagedResults()
    {
        var entry = new AuditHistory { Id = 1 };
        var response = new AuditHistoryResponse { Id = 1, Action = "Create", Entity = "Product" };
        var paginatedList = new PaginatedList<AuditHistory>([entry], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetAuditHistoriesAsync(_businessId, null, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<AuditHistoryResponse>>(It.IsAny<List<AuditHistory>>()))
            .Returns([response]);

        var result = await _service.GetAuditHistoriesAsync(new AuditHistorySearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.Items[0].Id);
    }

    [Fact]
    public async Task GetAuditHistoriesAsync_ReturnsEmptyList_WhenNoEntries()
    {
        _repositoryMock.Setup(r => r.GetAuditHistoriesAsync(_businessId, null, null, 1, 10))
            .ReturnsAsync(new PaginatedList<AuditHistory>([], 0, 1, 10));
        _mapperMock.Setup(m => m.Map<List<AuditHistoryResponse>>(It.IsAny<List<AuditHistory>>()))
            .Returns([]);

        var result = await _service.GetAuditHistoriesAsync(new AuditHistorySearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }
}
