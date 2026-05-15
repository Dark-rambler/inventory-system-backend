using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Application.Services.CategoryService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CategoryRequest>> _validatorMock;
    private readonly CategoryService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CategoryRequest>>();
        _service = new CategoryService(_repositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
    }

    private static Category CreateCategory(int id = 1) => new()
    {
        Id = id,
        Name = "Test Category",
        Description = "A test category",
        CreatedAt = DateTime.UtcNow
    };

    private static CategoryRequest CreateRequest() => new()
    {
        Name = "Test Category",
        Description = "A test category"
    };

    [Fact]
    public async Task GetCategoriesAsync_ReturnsPagedResults()
    {
        var category = CreateCategory();
        var response = new CategoryResponse { Id = category.Id, Name = category.Name };
        var paginatedList = new PaginatedList<Category>([category], 1, 1, 10);

        _repositoryMock.Setup(r => r.GetCategoriesAsync(_businessId, null, 1, 10))
            .ReturnsAsync(paginatedList);
        _mapperMock.Setup(m => m.Map<List<CategoryResponse>>(It.IsAny<List<Category>>()))
            .Returns([response]);

        var result = await _service.GetCategoriesAsync(new CategorySearchParams(), _businessId);

        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(category.Name, result.Items[0].Name);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ReturnsCategory_WhenExists()
    {
        var category = CreateCategory();
        var response = new CategoryResponse { Id = category.Id, Name = category.Name };

        _repositoryMock.Setup(r => r.GetCategoryByIdAsync(category.Id, _businessId))
            .ReturnsAsync(category);
        _mapperMock.Setup(m => m.Map<CategoryResponse>(category))
            .Returns(response);

        var result = await _service.GetCategoryByIdAsync(category.Id, _businessId);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        _repositoryMock.Setup(r => r.GetCategoryByIdAsync(99, _businessId))
            .ReturnsAsync((Category?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetCategoryByIdAsync(99, _businessId));
    }

    [Fact]
    public async Task CreateCategoryAsync_CreatesAndReturnsCategory()
    {
        var request = CreateRequest();
        var category = CreateCategory();
        var response = new CategoryResponse { Id = category.Id, Name = category.Name };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CategoryRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mapperMock.Setup(m => m.Map<Category>(request))
            .Returns(category);
        _repositoryMock.Setup(r => r.CreateCategoryAsync(category))
            .ReturnsAsync(category);
        _mapperMock.Setup(m => m.Map<CategoryResponse>(category))
            .Returns(response);

        var result = await _service.CreateCategoryAsync(request, _businessId);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
    }

    [Fact]
    public async Task UpdateCategoryAsync_UpdatesCategory_WhenExists()
    {
        var category = CreateCategory(5);
        var request = CreateRequest();

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CategoryRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _repositoryMock.Setup(r => r.GetCategoryByIdAsync(5, _businessId))
            .ReturnsAsync(category);
        _mapperMock.Setup(m => m.Map(request, category));

        await _service.UpdateCategoryAsync(5, request, _businessId);

        _repositoryMock.Verify(r => r.UpdateCategoryAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        _repositoryMock.Setup(r => r.GetCategoryByIdAsync(99, _businessId))
            .ReturnsAsync((Category?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.UpdateCategoryAsync(99, CreateRequest(), _businessId));
    }

    [Fact]
    public async Task DeleteCategoryAsync_DeletesCategory_WhenExists()
    {
        var category = CreateCategory(3);

        _repositoryMock.Setup(r => r.GetCategoryByIdAsync(3, _businessId))
            .ReturnsAsync(category);

        await _service.DeleteCategoryAsync(3, _businessId);

        _repositoryMock.Verify(r => r.DeleteCategoryAsync(category), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_ThrowsKeyNotFoundException_WhenNotExists()
    {
        _repositoryMock.Setup(r => r.GetCategoryByIdAsync(99, _businessId))
            .ReturnsAsync((Category?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteCategoryAsync(99, _businessId));
    }
}
