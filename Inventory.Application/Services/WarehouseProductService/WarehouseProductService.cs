using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseProductDto;
using Inventory.Domain.Entities.Builders;

namespace Inventory.Application.Services.WarehouseProductService
{
    public class WarehouseProductService(IWarehouseRepository repository, IMapper mapper) : IWarehouseProductService
    {
        public async Task<PaginatedList<WarehouseProductResponse>> GetProductsByWarehousesAsync(Guid id, ProductSearchParams searchParams, Guid businessId)
        {
            await FindWarehouseById(id, businessId);
            var warehouseProducts = await repository.GetProductsByWarehousesAsync(id, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<WarehouseProductResponse>(
                mapper.Map<List<WarehouseProductResponse>>(warehouseProducts.Items),
                warehouseProducts.TotalCount,
                warehouseProducts.PageIndex,
                warehouseProducts.PageSize
            );
        }

        public async Task AddProductsToWarehouseAsync(Guid id, IEnumerable<WarehouseProductRequest> request, Guid businessId)
        {
            await FindWarehouseById(id, businessId);
            var warehouseProducts = request.Select(r => new WarehouseProductBuilder()
                .WithWarehouseId(id)
                .WithProductId(r.ProductId)
                .WithStock(r.Stock)
                .WithLowStock(r.LowStock)
                .Build()
            ).ToList();
            await repository.AddProductsToWarehouseAsync(warehouseProducts);
        }

        public async Task<PaginatedList<ProductResponse>> GetProductsDoesntExistByWarehouseAsync(Guid id, ProductSearchParams searchParams, Guid businessId)
        {
            await FindWarehouseById(id, businessId);
            var products = await repository.GetProductsDoesntExistByWarehouseAsync(id, businessId, searchParams.Page, searchParams.PageSize);
            return mapper.Map<PaginatedList<ProductResponse>>(products);
        }

        private async Task FindWarehouseById(Guid id, Guid businessId)
        {
            _ = await repository.GetWarehouseByIdAsync(id, businessId) ?? throw new KeyNotFoundException($"Warehouse with id {id} doesn't exist");
        }
    }
}
