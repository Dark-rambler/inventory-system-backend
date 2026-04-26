namespace Inventory.Domain.Entities
{
    public class BranchProduct
    {
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; } = default!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public double Price { get; set; }
        public int Stock { get; set; }
        public int LowStock { get; set; }
    }
}
