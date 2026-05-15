using Inventory.Domain.Enum;

namespace Inventory.Application.DataTransferObjects.InventoryMovementDto
{
    public class InventoryMovementSearchParams
    {
        public Guid? WarehouseId { get; set; }
        public Guid? BranchId { get; set; }
        public EnumMovementType? MovementType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
