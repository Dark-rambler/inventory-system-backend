namespace Inventory.Domain.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Warehouse Warehouse { get; set; } = default!;
        public Branch Branch { get; set; } = default!;
    }
}
