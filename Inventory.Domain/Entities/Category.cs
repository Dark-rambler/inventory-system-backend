namespace Inventory.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public Guid BusinessId { get; set; }
        public Business Business { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = [];
    }
}
