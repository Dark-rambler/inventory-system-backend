namespace Inventory.Application.DataTransferObjects.WarehouseDto
{
    public class WarehouseRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
    }
}
