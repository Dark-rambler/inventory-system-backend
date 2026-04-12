using Inventory.Domain.Enum;

namespace Inventory.Application.Patterns.Strategies.InventoryMovementStrategy
{
    public class MovementStrategyResolver(IEnumerable<IInventoryMovementStrategy> strategies)
    {
        public IInventoryMovementStrategy Resolve(MovementType type)
        {
            return strategies.FirstOrDefault(s => s.Type == type) 
                ?? throw new InvalidOperationException($"No strategy found for movement type: {type}");
        }
    }
}
