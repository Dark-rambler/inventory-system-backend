namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public class SaleRequest
    {
        public Guid? CustomerId { get; set; }
        public IEnumerable<SaleDetailRequest> SaleDetails { get; set; } = [];
    }
}
