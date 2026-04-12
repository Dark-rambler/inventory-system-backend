namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public class SaleDetailResponse
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Product { get; set; } = string.Empty;
    }
}
