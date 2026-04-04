namespace Inventory.Application.DataTransferObjects.LocationDto
{
    public class LocationRequest
    {
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
    }
}
