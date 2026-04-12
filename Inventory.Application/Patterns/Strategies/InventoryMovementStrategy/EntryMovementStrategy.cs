using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Enum;

namespace Inventory.Application.Patterns.Strategies.InventoryMovementStrategy
{
    public class EntryMovementStrategy : IInventoryMovementStrategy
    {
        public MovementType Type => MovementType.Entry;

        public Task ExecuteAsync(InventoryMovementRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
