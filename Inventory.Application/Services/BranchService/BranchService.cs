using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.BranchService
{
    public class BranchService(
        IBranchRepository repository,
        IMapper mapper,
        IValidator<BranchRequest> validator) : IBranchService
    {
        public async Task<PaginatedList<BranchResponse>> GetBranchesAsync(BranchSearchParams searchParams)
        {
            var paginatedBranches = await repository.GetBranchesAsync(searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<BranchResponse>(
                mapper.Map<List<BranchResponse>>(paginatedBranches.Items),
                paginatedBranches.TotalCount,
                paginatedBranches.PageIndex,
                paginatedBranches.PageSize
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
                paginatedBranchProducts.TotalCount,
                paginatedBranchProducts.PageIndex,
                paginatedBranchProducts.PageSize
            );
        }

        public async Task CreateSaleAsync(Guid id, SaleRequest request, Guid user)
        {
            var productIds = request.SaleDetails.Select(sd => sd.ProductId).ToList();
            var products = await repository.GetBranchProductsByProductIdsAsync(id, productIds);
            var createdAt = DateTime.UtcNow;
            var productsUpdated = request.SaleDetails.Select(sd =>
            {
                var product = products.First(p => p.BranchId == id && p.ProductId == sd.ProductId);
                if (product.Stock < sd.Quantity)
                    throw new InvalidOperationException($"Not enough stock for product {product.Product.Name}");
                product.Stock -= sd.Quantity;
                return product;
            }).ToList();

            var sale = new Sale()
            {
                BranchId = id,
                Total = request.SaleDetails.Sum(sd => sd.Quantity * products.First(p => p.BranchId == id && p.ProductId == sd.ProductId).Price),
                Date = createdAt,
                SellerId = user,
                SaleDetails = [.. request.SaleDetails.Select(sd => new SaleDetail()
                {
                    ProductId = sd.ProductId,
                    Quantity = sd.Quantity,
                    Price = products.First(p => p.BranchId == id && p.ProductId == sd.ProductId).Price
                })]
            };
            var intentoryMovements = request.SaleDetails.Select(sd => new InventoryMovement()
            {
                FromBranchId = id,
                ProductId = sd.ProductId,
                Quantity = sd.Quantity,
                CreatedAt = createdAt,
                Type = MovementType.Sale,
                UserId = user,
            }).ToList();
            await repository.CreateSaleAsync(sale, intentoryMovements, productsUpdated);
        }

        public async Task<PaginatedList<SaleResponse>> GetSalesByBranchAsync(Guid id, SaleSearchParams searchParams)
        {
            var paginatedSales = await repository.GetSalesByBranchAsync(id, searchParams.FromDate, searchParams.ToDate, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<SaleResponse>(
                mapper.Map<List<SaleResponse>>(paginatedSales.Items),
                paginatedSales.TotalCount,
                paginatedSales.PageIndex,
                paginatedSales.PageSize
            );
        }
    }
}
