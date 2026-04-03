using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.BranchService
{
    public class BranchService(IBranchRepository repository, IMapper mapper, IValidator<BranchRequest> validator) : IBranchService
    {
        public async Task<PaginatedList<BranchResponse>> GetBranchesAsync(BranchSearchParams searchParams)
        {
            var paginatedBranches = await repository.GetBranchesAsync(searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<BranchResponse>(
                mapper.Map<List<BranchResponse>>(paginatedBranches.Items),
                paginatedBranches.PageIndex,
                paginatedBranches.PageSize,
                paginatedBranches.TotalCount
            );
        }

        public async Task<BranchResponse> GetBranchByIdAsync(Guid id)
        {
            return mapper.Map<BranchResponse>(await FindBranchById(id));
        }

        public async Task<BranchResponse> CreateBranchAsync(BranchRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            return mapper.Map<BranchResponse>(await repository.CreateBranchAsync(mapper.Map<Branch>(request)));
        }

        public async Task UpdateBranchAsync(Guid id, BranchRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateBranchAsync(mapper.Map(request, await FindBranchById(id)));
        }

        public async Task DeleteBranchAsync(Guid id)
        {
            await repository.DeleteBranchAsync(await FindBranchById(id));
        }

        private async Task<Branch> FindBranchById(Guid id)
        {
            return await repository.GetBranchByIdAsync(id) ?? throw new KeyNotFoundException($"Branch with id {id} doesn't exist");
        }

        public async Task<PaginatedList<BranchProductResponse>> GetProductsByBranchAsync(Guid id, ProductSearchParams searchParams)
        {
            var paginatedBranchProducts = await repository.GetProductsByBranchAsync(id, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<BranchProductResponse>(
                mapper.Map<List<BranchProductResponse>>(paginatedBranchProducts.Items),
                paginatedBranchProducts.PageIndex,
                paginatedBranchProducts.PageSize,
                paginatedBranchProducts.TotalCount
            );
        }
    }
}
