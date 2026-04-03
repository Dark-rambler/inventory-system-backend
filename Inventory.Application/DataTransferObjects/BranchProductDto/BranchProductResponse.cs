using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public class BranchProductResponse
    {
        public ProductResponse Product { get; set; } = default!;
        public int Stock { get; set; }
    }
}
