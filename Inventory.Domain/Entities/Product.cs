namespace Inventory.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public int MeasureId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public Category Category { get; set; } = default!;
        public Measure Measure { get; set; } = default!;
        public ICollection<BranchProduct> BranchProducts { get; set; } = [];
        public ICollection<WarehouseProduct> WarehouseProducts { get; set; } = [];
    }
}
