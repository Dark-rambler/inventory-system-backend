namespace Inventory.Domain.Entities
{
    public class Business
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Category> Categories { get; set; } = [];
        public ICollection<Product> Products { get; set; } = [];
        public ICollection<Warehouse> Warehouses { get; set; } = [];
        public ICollection<Branch> Branches { get; set; } = [];
        public ICollection<User> Users { get; set; } = [];
        public ICollection<Customer> Customers { get; set; } = [];
        public ICollection<Purchase> Purchases { get; set; } = [];
        public ICollection<Sale> Sales { get; set; } = [];
        public ICollection<Provider> Providers { get; set; } = [];
        public ICollection<InventoryMovement> InventoryMovements { get; set; } = [];
    }
}
