using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Enum;

namespace Inventory.Application.Patterns.Strategies.InventoryMovementStrategy
{
    public interface IInventoryMovementStrategy
    {
        MovementType Type { get; }
        Task ExecuteAsync(InventoryMovementRequest request);
    }
}
