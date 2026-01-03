namespace Inventory.Application.DataTransferObjects.ProductDto
{
    public class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
    }
}
