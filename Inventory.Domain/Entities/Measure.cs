namespace Inventory.Domain.Entities
{
    public class Measure
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<Product> Products { get; set; } = [];
    }
}
