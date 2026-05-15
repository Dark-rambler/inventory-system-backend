namespace Inventory.Domain.Entities
{
    public class WarehouseProduct
    {
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Stock { get; set; }
        public int LowStock { get; set; }

        public void AddStock(int quantity) => Stock += quantity;

        public void ReduceStock(int quantity)
        {
            if (Stock < quantity)
                throw new InvalidOperationException("Insufficient stock for the exit movement.");
            Stock -= quantity;
        }
    }
}
