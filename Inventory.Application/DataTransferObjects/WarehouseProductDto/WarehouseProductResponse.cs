using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.DataTransferObjects.WarehouseProductDto
{
    public class WarehouseProductResponse
    {
        public ProductResponse Product { get; set; } = default!;
        public int Stock { get; set; }
    }
}
