using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BusinessDto;
using Inventory.Application.Services.BusinessService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class BusinessServiceTests
{
    private readonly Mock<IBusinessRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<BusinessRequest>> _validatorMock;
    private readonly BusinessService _service;

    public BusinessServiceTests()
    {
        _repositoryMock = new Mock<IBusinessRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<BusinessRequest>>();
        _service = new BusinessService(_repositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    private static Business CreateBusiness(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Business"
    };

    private static BusinessRequest CreateRequest() => new() { Name = "Test Business" };

    [Fact]
    public async Task GetBusinessesAsync_ReturnsPagedResults()
    {
        var business = CreateBusiness();
        var response = new BusinessResponse { Id = business.Id, Name = business.Name };
        var paginatedList = new PaginatedList<Business>([business], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetBusinessesAsync(null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<BusinessResponse>>(It.IsAny<List<Business>>()))
            .Returns([response]);

        var result = await _service.GetBusinessesAsync(new BusinessSearchParams());

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(business.Name, result.Items[0].Name);
    }

    [Fact]
    public async Task CreateBusinessAsync_CreatesAndReturnsBusiness()
    {
        var request = CreateRequest();
        var business = CreateBusiness();
        var response = new BusinessResponse { Id = business.Id, Name = business.Name };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<BusinessRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<Business>(request))
            .Returns(business);
        _repositoryMock.Setup(r => r.CreateBusinessAsync(business))
            .ReturnsAsync(business);
        _mapperMock.Setup(m => m.Map<BusinessResponse>(business))
            .Returns(response);

        var result = await _service.CreateBusinessAsync(request);

        Assert.NotNull(result);
        Assert.Equal(business.Id, result.Id);
        Assert.Equal(business.Name, result.Name);
    }
}
