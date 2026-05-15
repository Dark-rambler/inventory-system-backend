namespace Inventory.Application.DataTransferObjects.ProductDto
{
    public class ProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int? MeasureId { get; set; }
    }
}
