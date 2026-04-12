namespace Inventory.Domain.Entities
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Guid SaleId { get; set; }
        public Sale Sale { get; set; } = default!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}
