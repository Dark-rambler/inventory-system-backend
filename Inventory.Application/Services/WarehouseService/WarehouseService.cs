using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Domain.Entities;

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

        public async Task<WarehouseResponse> GetWarehouseByIdAsync(Guid id, Guid businessId)
        {
            return mapper.Map<WarehouseResponse>(await FindWarehouseById(id, businessId));
        }

        public async Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest request, Guid businessId)
        {
            var warehouse = mapper.Map<Warehouse>(request);
            warehouse.BusinessId = businessId;
            return mapper.Map<WarehouseResponse>(await repository.CreateWarehouseAsync(warehouse));
        }

        public async Task UpdateWarehouseAsync(Guid id, WarehouseRequest request, Guid businessId)
        {
            await repository.UpdateWarehouseAsync(mapper.Map(request, await FindWarehouseById(id, businessId)));
        }

        public async Task DeleteWarehouseAsync(Guid id, Guid businessId)
        {
            await repository.DeleteWarehouseAsync(await FindWarehouseById(id, businessId));
        }

        private async Task<Warehouse> FindWarehouseById(Guid id, Guid businessId)
        {
            return await repository.GetWarehouseByIdAsync(id, businessId) ?? throw new KeyNotFoundException($"Warehouse with id {id} doesn't exist");
        }
    }
}
