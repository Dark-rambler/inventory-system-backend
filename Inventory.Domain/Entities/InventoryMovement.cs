using Inventory.Domain.Enum;

namespace Inventory.Domain.Entities
{
    public class InventoryMovement
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
        public MovementType Type { get; set; }
        public Guid? FromWarehouseId { get; set; }
        public Warehouse? FromWarehouse { get; set; }
        public Guid? FromBranchId { get; set; }
        public Branch? FromBranch { get; set; }
        public Guid? ToWarehouseId { get; set; }
        public Warehouse? ToWarehouse { get; set; }
        public Guid? ToBranchId { get; set; }
        public Branch? ToBranch { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
