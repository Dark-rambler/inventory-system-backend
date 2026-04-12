namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public class SaleRequest
    {
        public IEnumerable<SaleDetailRequest> SaleDetails { get; set; } = [];
    }
}
