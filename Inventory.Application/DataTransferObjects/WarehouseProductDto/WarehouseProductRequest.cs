namespace Inventory.Application.DataTransferObjects.WarehouseProductDto
{
    public class WarehouseProductRequest
    {
        public int ProductId { get; set; }
        public int Stock { get; set; }
        public int LowStock { get; set; }
    }
}
