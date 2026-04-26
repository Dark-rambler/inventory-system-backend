namespace Inventory.Application.DataTransferObjects.PurchaseDto
{
    public class PurchaseDetailRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}