namespace Inventory.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
