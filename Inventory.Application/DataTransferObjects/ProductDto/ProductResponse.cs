using Inventory.Application.DataTransferObjects.CategoryDto;

namespace Inventory.Application.DataTransferObjects.ProductDto
{
    public class ProductResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CategoryResponse Category { get; set; } = default!;
    }
}
