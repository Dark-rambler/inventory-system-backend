using Inventory.Application.DataTransferObjects.Location;

namespace Inventory.Application.DataTransferObjects.WarehouseDto
{
    public class WarehouseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public LocationResponse Location { get; set; } = default!;
    }
}
