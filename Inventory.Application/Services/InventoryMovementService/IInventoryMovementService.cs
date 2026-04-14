using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;

namespace Inventory.Application.Services.InventoryMovementService
{
    public interface IInventoryMovementService
    {
        Task<InventoryMovementResponse> CreateInventoryMovementAsync(InventoryMovementRequest request, Guid user);
        Task<PaginatedList<InventoryMovementResponse>> GetInventoryMovementsAsync(InventoryMovementSearchParams searchParams);
    }
}
