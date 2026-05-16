using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.BranchService
{
    public class BranchService(
        IBranchRepository repository,
        IMapper mapper,
        IValidator<BranchRequest> validator) : IBranchService
    {
        public async Task<PaginatedList<BranchResponse>> GetBranchesAsync(BranchSearchParams searchParams, Guid businessId)
        {
            var paginatedBranches = await repository.GetBranchesAsync(businessId, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<BranchResponse>(
                mapper.Map<List<BranchResponse>>(paginatedBranches.Items),
                paginatedBranches.TotalCount,
                paginatedBranches.PageIndex,
                paginatedBranches.PageSize
            );
        }

        public async Task<BranchResponse> GetBranchByIdAsync(Guid id, Guid businessId)
        {
            return mapper.Map<BranchResponse>(await FindBranchById(id, businessId));
        }

        public async Task<BranchResponse> CreateBranchAsync(BranchRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            var branch = mapper.Map<Branch>(request);
            branch.BusinessId = businessId;
            return mapper.Map<BranchResponse>(await repository.CreateBranchAsync(branch));
        }

        public async Task UpdateBranchAsync(Guid id, BranchRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateBranchAsync(mapper.Map(request, await FindBranchById(id, businessId)));
        }

        public async Task DeleteBranchAsync(Guid id, Guid businessId)
        {
            await repository.DeleteBranchAsync(await FindBranchById(id, businessId));
        }

        private async Task<Branch> FindBranchById(Guid id, Guid businessId)
        {
            return await repository.GetBranchByIdAsync(id, businessId) ?? throw new KeyNotFoundException($"Branch with id {id} doesn't exist");
        }
    }
}
