using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Entities.Builders;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.BranchService
{
    public class BranchService(
        IBranchRepository repository,
        IMapper mapper,
        IValidator<BranchRequest> validator,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider) : IBranchService
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

        public async Task UpdateBranchProductAsync(Guid id, BranchProductRequest request, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var product = await FindBranchProduct(id, request.ProductId);
            await repository.UpdateBranchProductAsync(mapper.Map(request, product));
        }

        public async Task DeleteBranchAsync(Guid id, Guid businessId)
        {
            await repository.DeleteBranchAsync(await FindBranchById(id, businessId));
        }

        public async Task DeleteProductsAsync(Guid branchId, IEnumerable<int> productIds, Guid businessId)
        {
            await FindBranchById(branchId, businessId);
            var products = await repository.GetBranchProductsByProductIdsAsync(branchId, productIds);
            await repository.DeleteProductsAsync(products);
        }

        private async Task<Branch> FindBranchById(Guid id, Guid businessId)
        {
            return await repository.GetBranchByIdAsync(id, businessId) ?? throw new KeyNotFoundException($"Branch with id {id} doesn't exist");
        }

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

        public async Task CreateSaleAsync(Guid id, SaleRequest request, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var user = currentUserService.GetCurrentUserId();
            var productIds = request.SaleDetails.Select(sd => sd.ProductId).ToList();
            var products = await repository.GetBranchProductsByProductIdsAsync(id, productIds);
            var createdAt = dateTimeProvider.UtcNow;

            var productsUpdated = request.SaleDetails.Select(sd =>
            {
                var product = products.First(p => p.BranchId == id && p.ProductId == sd.ProductId);
                product.ReduceStock(sd.Quantity);
                return product;
            }).ToList();

            var sale = new SaleBuilder()
                .WithBusinessId(businessId)
                .WithBranchId(id)
                .WithSellerId(user)
                .WithCustomerId(request.CustomerId)
                .WithDate(createdAt)
                .WithTotal(request.SaleDetails.Sum(sd => sd.Quantity * products.First(p => p.BranchId == id && p.ProductId == sd.ProductId).Price))
                .WithSaleDetails([.. request.SaleDetails.Select(sd => new SaleDetailBuilder()
                    .WithProductId(sd.ProductId)
                    .WithQuantity(sd.Quantity)
                    .WithPrice(products.First(p => p.BranchId == id && p.ProductId == sd.ProductId).Price)
                    .Build())])
                .Build();

            var inventoryMovements = request.SaleDetails.Select(sd => new InventoryMovementBuilder()
                .WithProductId(sd.ProductId)
                .WithQuantity(sd.Quantity)
                .WithType(EnumMovementType.Exit)
                .WithIsSale(true)
                .WithFromBranchId(id)
                .WithUserId(user)
                .WithCreatedAt(createdAt)
                .Build()
            ).ToList();

            var auditHistory = new AuditHistoryBuilder()
                .WithAction(EnumAction.Create)
                .WithEntity(EnumEntity.Sale)
                .WithUserId(user)
                .WithBusinessId(businessId)
                .WithCreatedAt(createdAt)
                .WithDescription($"Sale created with total {sale.Total}")
                .Build();

            await repository.CreateSaleAsync(sale, inventoryMovements, productsUpdated, auditHistory);
        }

        public async Task<PaginatedList<SaleResponse>> GetSalesByBranchAsync(Guid id, SaleSearchParams searchParams, Guid businessId)
        {
            await FindBranchById(id, businessId);
            var paginatedSales = await repository.GetSalesByBranchAsync(businessId, id, searchParams.FromDate, searchParams.ToDate, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<SaleResponse>(
                mapper.Map<List<SaleResponse>>(paginatedSales.Items),
                paginatedSales.TotalCount,
                paginatedSales.PageIndex,
                paginatedSales.PageSize
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

        private async Task<BranchProduct> FindBranchProduct(Guid branchId, int productId)
        {
            var branchProduct = await repository.GetBranchProductByBranchIdAndProductIdAsync(branchId, productId) ?? throw new KeyNotFoundException($"Product with id {productId} doesn't exist in branch with id {branchId}");
            return branchProduct;
        }
    }
}