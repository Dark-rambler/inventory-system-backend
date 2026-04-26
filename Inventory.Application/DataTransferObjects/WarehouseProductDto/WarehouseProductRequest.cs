namespace Inventory.Application.DataTransferObjects.WarehouseProductDto
{
    public class WarehouseProductRequest
    {
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
        public int LowStock { get; set; }
    }
}
