using Inventory.Domain.Enum;

namespace Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy
{
    public class MovementStrategyResolver(IEnumerable<IInventoryMovementStrategy> strategies)
    {
        public IInventoryMovementStrategy Resolve(EnumMovementType type)
        {
            return strategies.FirstOrDefault(s => s.Type == type) 
                ?? throw new InvalidOperationException($"No strategy found for movement type: {type}");
        }
    }
}
