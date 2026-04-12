using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Enum;

namespace Inventory.Application.Patterns.Strategies.InventoryMovementStrategy
{
    public class TransferMovementStrategy : IInventoryMovementStrategy
    {
        public MovementType Type => MovementType.Transfer;

        public Task ExecuteAsync(InventoryMovementRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
