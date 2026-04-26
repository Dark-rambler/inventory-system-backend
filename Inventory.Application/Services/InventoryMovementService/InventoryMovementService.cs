using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy;

namespace Inventory.Application.Services.InventoryMovementService
{
    public class InventoryMovementService(IInventoryMovementRepository repository, MovementStrategyResolver resolver, IMapper mapper) : IInventoryMovementService
    {
        public async Task<InventoryMovementResponse> CreateInventoryMovementAsync(InventoryMovementRequest request, Guid user)
        {
            var strategy = resolver.Resolve(request.Type);
            return mapper.Map<InventoryMovementResponse>(await strategy.ExecuteAsync(request, repository, mapper, user));
        }

        public async Task<PaginatedList<InventoryMovementResponse>> GetInventoryMovementsAsync(InventoryMovementSearchParams searchParams)
        {
            var paginatedInventoryMovements = await repository.GetInventoryMovementsAsync(searchParams.WarehouseId, searchParams.BranchId, searchParams.MovementType, searchParams.FromDate, searchParams.ToDate, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<InventoryMovementResponse>(mapper.Map<List<InventoryMovementResponse>>(paginatedInventoryMovements.Items),
                paginatedInventoryMovements.TotalCount,
                paginatedInventoryMovements.PageIndex,
                paginatedInventoryMovements.PageSize
            );
        }
    }
}
