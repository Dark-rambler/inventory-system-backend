namespace Inventory.Domain.Entities
{
    public class Warehouse
    {
        public Guid  Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public Branch Branch { get; set; } = default!;
    }
}
