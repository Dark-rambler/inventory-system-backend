using Inventory.Domain.Enum;

namespace Inventory.Application.DataTransferObjects.InventoryMovementDto
{
    public class InventoryMovementRequest
    {
        public class InventoryMovementDto
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
            public MovementType Type { get; set; }
            public Guid? FromWarehouseId { get; set; }
            public Guid? ToWarehouseId { get; set; }
            public Guid? FromBranchId { get; set; }
            public Guid? ToBranchId { get; set; }
        }
    }
}
