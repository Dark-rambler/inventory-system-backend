namespace Inventory.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Folio { get; set; } = string.Empty;
        public double Total { get; set; }
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; } = default!;
        public Guid SellerId { get; set; }
        public User Seller { get; set; } = default!;
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<SaleDetail> SaleDetails { get; set; } = [];
    }
}
