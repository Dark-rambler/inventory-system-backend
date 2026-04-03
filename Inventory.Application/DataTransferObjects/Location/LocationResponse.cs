namespace Inventory.Application.DataTransferObjects.Location
{
    public class LocationResponse
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
