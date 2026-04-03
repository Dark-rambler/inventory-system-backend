using Inventory.Application.DataTransferObjects.Location;

namespace Inventory.Application.DataTransferObjects.WarehouseDto
{
    public class WarehouseRequest
    {
        public string Name { get; set; } = string.Empty;
        public LocationRequest Location { get; set; } = default!;
    }
}
