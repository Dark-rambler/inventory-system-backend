namespace Inventory.Domain.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public Guid BusinessId { get; set; }
        public Business Business { get; set; } = null!;
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; } = default!;
        public Guid BuyerId { get; set; }
        public User Buyer { get; set; } = default!;
        public Guid? BranchId { get; set; }
        public Branch? Branch { get; set; } = null;
        public Guid? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; } = null;
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = [];
    }
}