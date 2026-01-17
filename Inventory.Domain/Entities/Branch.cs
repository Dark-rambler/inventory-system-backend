namespace Inventory.Domain.Entities
{
    public class Branch
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public ICollection<Warehouse> Warehouses { get; set; } = [];
    }
}
