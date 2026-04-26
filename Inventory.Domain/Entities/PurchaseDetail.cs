namespace Inventory.Domain.Entities
{
    public class PurchaseDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Guid PurchaseId { get; set; }
        public Purchase Purchase { get; set; } = default!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}