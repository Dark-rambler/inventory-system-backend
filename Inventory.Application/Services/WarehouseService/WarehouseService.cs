using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.WarehouseService
{
    public class WarehouseService(IWarehouseRepository repository, IMapper mapper) : IWarehouseService
    {
        public async Task<PaginatedList<WarehouseResponse>> GetWarehousesAsync(WarehouseSearchParams searchParams)
        {
            var warehouses = await repository.GetWarehousesAsync(searchParams.Name, searchParams.Page, searchParams.PageSize);
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

        public async Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest request)
        {
            return mapper.Map<WarehouseResponse>(await repository.CreateWarehouseAsync(mapper.Map<Warehouse>(request)));
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

        public async Task<PaginatedList<WarehouseResponse>> GetProductsByWarehousesAsync(Guid id, ProductSearchParams searchParams)
        {
            var warehouseProducts = await repository.GetProductsByWarehousesAsync(id, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<WarehouseResponse>(
                mapper.Map<List<WarehouseResponse>>(warehouseProducts.Items),
                warehouseProducts.TotalCount,
                warehouseProducts.PageIndex,
                warehouseProducts.PageSize
            );
        }
    }
}
