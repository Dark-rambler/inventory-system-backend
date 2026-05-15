namespace Inventory.Application.DataTransferObjects.PurchaseDto
{
    public class PurchaseDetailRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}