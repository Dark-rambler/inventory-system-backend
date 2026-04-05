namespace Inventory.Application.DataTransferObjects.LocationDto
{
    public class LocationRequest
    {
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
    }
}
