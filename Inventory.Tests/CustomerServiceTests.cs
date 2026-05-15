using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CustomerDto;
using Inventory.Application.Services.CustomerService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CustomerRequest>> _validatorMock;
    private readonly CustomerService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public CustomerServiceTests()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CustomerRequest>>();
        _service = new CustomerService(_repositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    private static Customer CreateCustomer(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Customer",
        Nit = "123456789",
        Phone = "5551234567",
        CreatedAt = DateTime.UtcNow
    };

    private static CustomerRequest CreateRequest() => new()
    {
        Name = "Test Customer",
        Nit = "123456789",
        Phone = "5551234567"
    };

    [Fact]
    public async Task GetCustomersAsync_ReturnsPagedResults()
    {
        var customer = CreateCustomer();
        var response = new CustomerResponse { Id = customer.Id, Name = customer.Name };
        var paginatedList = new PaginatedList<Customer>([customer], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetCustomersAsync(_businessId, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<CustomerResponse>>(It.IsAny<List<Customer>>()))
            .Returns([response]);

        var result = await _service.GetCustomersAsync(new CustomerSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(customer.Name, result.Items[0].Name);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsCustomer_WhenExists()
    {
        var customer = CreateCustomer();
        var response = new CustomerResponse { Id = customer.Id, Name = customer.Name };

        _repositoryMock.Setup(r => r.GetCustomerByIdAsync(customer.Id, _businessId))
            .ReturnsAsync(customer);
        _mapperMock.Setup(m => m.Map<CustomerResponse>(customer))
            .Returns(response);

        var result = await _service.GetCustomerByIdAsync(customer.Id, _businessId);

        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var customerId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetCustomerByIdAsync(customerId, _businessId))
            .ReturnsAsync((Customer?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetCustomerByIdAsync(customerId, _businessId));
    }

    [Fact]
    public async Task CreateCustomerAsync_CreatesAndReturnsCustomer()
    {
        var request = CreateRequest();
        var customer = CreateCustomer();
        var response = new CustomerResponse { Id = customer.Id, Name = customer.Name };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CustomerRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<Customer>(request))
            .Returns(customer);
        _repositoryMock.Setup(r => r.CreateCustomerAsync(customer))
            .ReturnsAsync(customer);
        _mapperMock.Setup(m => m.Map<CustomerResponse>(customer))
            .Returns(response);

        var result = await _service.CreateCustomerAsync(request, _businessId);

        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
    }

    [Fact]
    public async Task UpdateCustomerAsync_UpdatesCustomer_WhenExists()
    {
        var customerId = Guid.NewGuid();
        var request = CreateRequest();
        var customer = CreateCustomer(customerId);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CustomerRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetCustomerByIdAsync(customerId, _businessId))
            .ReturnsAsync(customer);
        _mapperMock.Setup(m => m.Map(request, customer));

        await _service.UpdateCustomerAsync(customerId, request, _businessId);

        _repositoryMock.Verify(r => r.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        var customerId = Guid.NewGuid();
        var request = CreateRequest();

        _repositoryMock.Setup(r => r.GetCustomerByIdAsync(customerId, _businessId))
            .ReturnsAsync((Customer?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateCustomerAsync(customerId, request, _businessId));
    }
}
