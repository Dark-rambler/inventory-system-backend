using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.Services.BranchService;
using Inventory.Domain.Entities;
using Moq;

namespace Inventory.Tests;

public class BranchServiceTests
{
    private readonly Mock<IBranchRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<BranchRequest>> _validatorMock;
    private readonly BranchService _service;
    private readonly Guid _businessId = Guid.NewGuid();

    public BranchServiceTests()
    {
        _repositoryMock = new Mock<IBranchRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<BranchRequest>>();
        _service = new BranchService(
            _repositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object);
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
}
