namespace Inventory.Domain.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; } = default!;
        public Guid BuyerId { get; set; }
        public User Buyer { get; set; } = default!;
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = [];
    }
}