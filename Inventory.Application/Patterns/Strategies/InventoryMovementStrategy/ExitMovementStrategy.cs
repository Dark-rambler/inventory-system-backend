using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Enum;

namespace Inventory.Application.Patterns.Strategies.InventoryMovementStrategy
{
    public class ExitMovementStrategy : IInventoryMovementStrategy
    {
        public MovementType Type => MovementType.Exit;

        public Task ExecuteAsync(InventoryMovementRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
