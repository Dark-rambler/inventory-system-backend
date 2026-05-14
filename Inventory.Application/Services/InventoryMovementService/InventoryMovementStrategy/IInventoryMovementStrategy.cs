using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy
{
    public interface IInventoryMovementStrategy
    {
        EnumMovementType Type { get; }
        Task<InventoryMovement> ExecuteAsync(InventoryMovementRequest request, Guid businessId);
    }
}
