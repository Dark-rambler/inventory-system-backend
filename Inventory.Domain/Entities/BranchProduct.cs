namespace Inventory.Domain.Entities
{
    public class BranchProduct
    {
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public double Price { get; set; }
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
