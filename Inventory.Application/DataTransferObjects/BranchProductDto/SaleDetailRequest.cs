namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public class SaleDetailRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
