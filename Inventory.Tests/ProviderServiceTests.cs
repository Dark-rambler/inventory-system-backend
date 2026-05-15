using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProviderDto;
using Inventory.Application.Services.ProviderService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class ProviderServiceTests
{
    private readonly Mock<IProviderRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<ProviderRequest>> _validatorMock;
    private readonly ProviderService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public ProviderServiceTests()
    {
        _repositoryMock = new Mock<IProviderRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<ProviderRequest>>();
        _service = new ProviderService(_repositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    private static Provider CreateProvider(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Provider",
        Contact = "John Doe",
        Telephone = "1234567890",
        Email = "test@provider.com",
        City = "Test City",
        CreatedAt = DateTime.UtcNow
    };

    private static ProviderRequest CreateRequest() => new()
    {
        Name = "Test Provider",
        Contact = "John Doe",
        Telephone = "1234567890",
        Email = "test@provider.com",
        City = "Test City"
    };

    [Fact]
    public async Task GetProvidersAsync_ReturnsPagedResults()
    {
        var provider = CreateProvider();
        var response = new ProviderResponse { Id = provider.Id, Name = provider.Name };
        var paginatedList = new PaginatedList<Provider>(new List<Provider> { provider }, 1, 1, 10);

        _repositoryMock.Setup(r => r.GetProvidersAsync(_businessId, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<ProviderResponse>>(It.IsAny<List<Provider>>()))
            .Returns([response]);

        var result = await _service.GetProvidersAsync(new ProviderSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(provider.Name, result.Items[0].Name);
    }

    [Fact]
    public async Task GetProviderByIdAsync_ReturnsProvider_WhenExists()
    {
        var provider = CreateProvider();
        var response = new ProviderResponse { Id = provider.Id, Name = provider.Name };

        _repositoryMock.Setup(r => r.GetProviderByIdAsync(provider.Id, _businessId))
            .ReturnsAsync(provider);
        _mapperMock.Setup(m => m.Map<ProviderResponse>(provider))
            .Returns(response);

        var result = await _service.GetProviderByIdAsync(provider.Id, _businessId);

        Assert.NotNull(result);
        Assert.Equal(provider.Id, result.Id);
    }

    [Fact]
    public async Task GetProviderByIdAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var providerId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetProviderByIdAsync(providerId, _businessId))
            .ReturnsAsync((Provider?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetProviderByIdAsync(providerId, _businessId));
    }

    [Fact]
    public async Task CreateProviderAsync_CreatesAndReturnsProvider()
    {
        var request = CreateRequest();
        var provider = CreateProvider();
        var response = new ProviderResponse { Id = provider.Id, Name = provider.Name };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ProviderRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<Provider>(request))
            .Returns(provider);
        _repositoryMock.Setup(r => r.CreateProviderAsync(provider))
            .ReturnsAsync(provider);
        _mapperMock.Setup(m => m.Map<ProviderResponse>(provider))
            .Returns(response);

        var result = await _service.CreateProviderAsync(request, _businessId);

        Assert.NotNull(result);
        Assert.Equal(provider.Id, result.Id);
    }

    [Fact]
    public async Task UpdateProviderAsync_UpdatesProvider_WhenExists()
    {
        var providerId = Guid.NewGuid();
        var request = CreateRequest();
        var provider = CreateProvider(providerId);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ProviderRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetProviderByIdAsync(providerId, _businessId))
            .ReturnsAsync(provider);
        _mapperMock.Setup(m => m.Map(request, provider));

        await _service.UpdateProviderAsync(providerId, request, _businessId);

        _repositoryMock.Verify(r => r.UpdateProviderAsync(It.IsAny<Provider>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProviderAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var providerId = Guid.NewGuid();
        var request = CreateRequest();

        _repositoryMock.Setup(r => r.GetProviderByIdAsync(providerId, _businessId))
            .ReturnsAsync((Provider?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateProviderAsync(providerId, request, _businessId));
    }

    [Fact]
    public async Task DeleteProviderAsync_DeletesProvider_WhenExists()
    {
        var provider = CreateProvider();

        _repositoryMock.Setup(r => r.GetProviderByIdAsync(provider.Id, _businessId))
            .ReturnsAsync(provider);

        await _service.DeleteProviderAsync(provider.Id, _businessId);

        _repositoryMock.Verify(r => r.DeleteProviderAsync(provider), Times.Once);
    }

    [Fact]
    public async Task DeleteProviderAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var providerId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetProviderByIdAsync(providerId, _businessId))
            .ReturnsAsync((Provider?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteProviderAsync(providerId, _businessId));
    }
}