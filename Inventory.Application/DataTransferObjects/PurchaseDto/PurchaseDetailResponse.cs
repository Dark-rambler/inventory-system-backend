namespace Inventory.Application.DataTransferObjects.PurchaseDto
{
    public class PurchaseDetailResponse
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Product { get; set; } = string.Empty;
    }
}