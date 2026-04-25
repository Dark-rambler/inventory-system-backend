namespace Inventory.Application.DataTransferObjects.ProviderDto
{
    public class ProviderRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}