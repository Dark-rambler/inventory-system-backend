using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Entities.Builders;

namespace Inventory.Application.Services.BranchProductService
{
    public class BranchProductService(IBranchRepository repository, IMapper mapper) : IBranchProductService
    {
        public async Task<PaginatedList<BranchProductResponse>> GetProductsByBranchAsync(Guid id, ProductSearchParams searchParams, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var paginatedBranchProducts = await repository.GetProductsByBranchAsync(id, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<BranchProductResponse>(
                mapper.Map<List<BranchProductResponse>>(paginatedBranchProducts.Items),
                paginatedBranchProducts.TotalCount,
                paginatedBranchProducts.PageIndex,
                paginatedBranchProducts.PageSize
            );
        }

        public async Task AddProductsToBranchAsync(Guid id, IEnumerable<BranchProductRequest> request, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var branchProducts = request.Select(r => new BranchProductBuilder()
                .WithBranchId(id)
                .WithProductId(r.ProductId)
                .WithPrice(r.Price)
                .WithStock(r.Stock)
                .WithLowStock(r.LowStock)
                .Build()
            ).ToList();
            await repository.AddProductsToBranchAsync(branchProducts);
        }

        public async Task DeleteProductsAsync(Guid branchId, IEnumerable<int> productIds, Guid businessId)
        {
            await FindBranchById(branchId, businessId);
            var products = await repository.GetBranchProductsByProductIdsAsync(branchId, productIds);
            await repository.DeleteProductsAsync(products);
        }

        public async Task<PaginatedList<ProductResponse>> GetProductsDoesntExistByBranchAsync(Guid id, ProductSearchParams searchParams, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var products = await repository.GetProductsDoesntExistByBranchAsync(id, businessId, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<ProductResponse>(
                mapper.Map<List<ProductResponse>>(products.Items),
                products.TotalCount,
                products.PageIndex,
                products.PageSize
            );
        }

        public async Task UpdateBranchProductAsync(Guid id, BranchProductRequest request, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var product = await FindBranchProduct(id, request.ProductId);
            await repository.UpdateBranchProductAsync(mapper.Map(request, product));
        }

        private async Task<Branch> FindBranchById(Guid id, Guid businessId)
        {
            return await repository.GetBranchByIdAsync(id, businessId) ?? throw new KeyNotFoundException($"Branch with id {id} doesn't exist");
        }

        private async Task<BranchProduct> FindBranchProduct(Guid branchId, int productId)
        {
            return await repository.GetBranchProductByBranchIdAndProductIdAsync(branchId, productId) ?? throw new KeyNotFoundException($"Product with id {productId} doesn't exist in branch with id {branchId}");
        }
    }
}
