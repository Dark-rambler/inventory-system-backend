namespace Inventory.Application.DataTransferObjects.CustomerDto
{
    public class CustomerRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Nit { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}