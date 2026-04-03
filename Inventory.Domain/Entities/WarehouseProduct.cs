namespace Inventory.Domain.Entities
{
    public class WarehouseProduct
    {
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = default!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Stock { get; set; }
    }
}
