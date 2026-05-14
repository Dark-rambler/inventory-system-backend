using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Abstracts.Clients;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.Services.ProductService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IExcelReader> _excelReaderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<ProductRequest>> _validatorMock;
    private readonly ProductService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _excelReaderMock = new Mock<IExcelReader>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<ProductRequest>>();
        _service = new ProductService(
            _repositoryMock.Object,
            _excelReaderMock.Object,
            _mapperMock.Object,
            _validatorMock.Object);
    }

    private static Product CreateProduct(int id = 1) => new()
    {
        Id = id,
        Name = "Test Product",
        Code = "TST-001",
        Description = "A test product",
        CategoryId = 1,
        CreatedAt = DateTime.UtcNow
    };

    private static ProductRequest CreateRequest() => new()
    {
        Name = "Test Product",
        Code = "TST-001",
        Description = "A test product",
        CategoryId = 1
    };

    [Fact]
    public async Task GetProductsAsync_ReturnsPagedResults()
    {
        var product = CreateProduct();
        var response = new ProductResponse { Id = product.Id, Name = product.Name };
        var paginatedList = new PaginatedList<Product>([product], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetProductsAsync(_businessId, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
            .Returns([response]);

        var result = await _service.GetProductsAsync(new ProductSearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(product.Name, result.Items[0].Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsProduct_WhenExists()
    {
        var product = CreateProduct();
        var response = new ProductResponse { Id = product.Id, Name = product.Name };

        _repositoryMock.Setup(r => r.GetProductByIdAsync(product.Id)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductResponse>(product)).Returns(response);

        var result = await _service.GetProductByIdAsync(product.Id);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
    }

    [Fact]
    public async Task GetProductByIdAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        _repositoryMock.Setup(r => r.GetProductByIdAsync(99))
            .ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetProductByIdAsync(99));
    }

    [Fact]
    public async Task CreateProductAsync_CreatesAndReturnsProduct()
    {
        var request = CreateRequest();
        var product = CreateProduct();
        var response = new ProductResponse { Id = product.Id, Name = product.Name };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<ProductRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<Product>(request)).Returns(product);
        _repositoryMock.Setup(r => r.CreateProductAsync(product)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductResponse>(product)).Returns(response);

        var result = await _service.CreateProductAsync(request, _businessId);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
    }

    [Fact]
    public async Task UpdateProductAsync_UpdatesProduct_WhenExists()
    {
        var product = CreateProduct(5);
        var request = CreateRequest();

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<ProductRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetProductByIdAsync(5)).ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map(request, product));

        await _service.UpdateProductAsync(5, request);

        _repositoryMock.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        _repositoryMock.Setup(r => r.GetProductByIdAsync(99))
            .ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateProductAsync(99, CreateRequest()));
    }

    [Fact]
    public async Task DeleteProductAsync_DeletesProduct_WhenExists()
    {
        var product = CreateProduct(3);

        _repositoryMock.Setup(r => r.GetProductByIdAsync(3)).ReturnsAsync(product);

        await _service.DeleteProductAsync(3);

        _repositoryMock.Verify(r => r.DeleteProductAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        _repositoryMock.Setup(r => r.GetProductByIdAsync(99))
            .ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteProductAsync(99));
    }

    [Fact]
    public async Task BulkUploadProductsAsync_ReadsAndBulkCreatesProducts()
    {
        var products = new List<Product> { CreateProduct(1), CreateProduct(2) };
        using var stream = new MemoryStream();

        _excelReaderMock.Setup(e => e.ReadProductsAsync(stream)).ReturnsAsync(products);
        _repositoryMock.Setup(r => r.BulkCreateAsync(products)).ReturnsAsync(products);

        await _service.BulkUploadProductsAsync(stream);

        _excelReaderMock.Verify(e => e.ReadProductsAsync(stream), Times.Once);
        _repositoryMock.Verify(r => r.BulkCreateAsync(products), Times.Once);
    }

    [Fact]
    public void GetBulkUploadTemplate_ReturnsStream()
    {
        using var expectedStream = new MemoryStream();

        _excelReaderMock.Setup(e => e.GenerateProductsTemplate()).Returns(expectedStream);

        var result = _service.GetBulkUploadTemplate();

        Assert.NotNull(result);
        Assert.Same(expectedStream, result);
    }
}
