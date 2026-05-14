namespace Inventory.Domain.Entities
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid BusinessId { get; set; }
        public Business Business { get; set; } = null!;
        public Location Location { get; set; } = default!;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public ICollection<WarehouseProduct> WarehouseProducts { get; set; } = [];
    }
}
