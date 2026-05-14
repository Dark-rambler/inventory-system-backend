using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Application.DataTransferObjects.WarehouseProductDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Entities.Builders;

namespace Inventory.Application.Services.WarehouseService
{
    public class WarehouseService(IWarehouseRepository repository, IMapper mapper) : IWarehouseService
    {
        public async Task<PaginatedList<WarehouseResponse>> GetWarehousesAsync(WarehouseSearchParams searchParams, Guid businessId)
        {
            var warehouses = await repository.GetWarehousesAsync(businessId, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<WarehouseResponse>(
                mapper.Map<List<WarehouseResponse>>(warehouses.Items),
                warehouses.TotalCount,
                warehouses.PageIndex,
                warehouses.PageSize
            );
        }

        public async Task<WarehouseResponse> GetWarehouseByIdAsync(Guid id)
        {
            return mapper.Map<WarehouseResponse>(await FindWarehouseById(id));
        }

        public async Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest request, Guid businessId)
        {
            var warehouse = mapper.Map<Warehouse>(request);
            warehouse.BusinessId = businessId;
            return mapper.Map<WarehouseResponse>(await repository.CreateWarehouseAsync(warehouse));
        }

        public async Task UpdateWarehouseAsync(Guid id, WarehouseRequest request)
        {
            await repository.UpdateWarehouseAsync(mapper.Map(request, await FindWarehouseById(id)));
        }

        public async Task DeleteWarehouseAsync(Guid id)
        {
            await repository.DeleteWarehouseAsync(await FindWarehouseById(id));
        }

        private async Task<Warehouse> FindWarehouseById(Guid id)
        {
            return await repository.GetWarehouseByIdAsync(id) ?? throw new KeyNotFoundException($"Warehouse with id {id} doesn't exist");
        }

        public async Task<PaginatedList<WarehouseProductResponse>> GetProductsByWarehousesAsync(Guid id, ProductSearchParams searchParams)
        {
            await FindWarehouseById(id);
            var warehouseProducts = await repository.GetProductsByWarehousesAsync(id, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<WarehouseProductResponse>(
                mapper.Map<List<WarehouseProductResponse>>(warehouseProducts.Items),
                warehouseProducts.TotalCount,
                warehouseProducts.PageIndex,
                warehouseProducts.PageSize
            );
        }

        public async Task AddProductsToWarehouseAsync(Guid id, IEnumerable<WarehouseProductRequest> request)
        {
            await FindWarehouseById(id);
            var warehouseProducts = request.Select(r => new WarehouseProductBuilder()
                .WithWarehouseId(id)
                .WithProductId(r.ProductId)
                .WithStock(r.Stock)
                .WithLowStock(r.LowStock)
                .Build()
            ).ToList();
            await repository.AddProductsToWarehouseAsync(warehouseProducts);
        }

        public async Task<PaginatedList<ProductResponse>> GetProductsDoesntExistByWarehouseAsync(Guid id, ProductSearchParams searchParams)
        {
            await FindWarehouseById(id);
            var products = await repository.GetProductsDoesntExistByWarehouseAsync(id, searchParams.Page, searchParams.PageSize);
            return mapper.Map<PaginatedList<ProductResponse>>(products);
        }
    }
}
