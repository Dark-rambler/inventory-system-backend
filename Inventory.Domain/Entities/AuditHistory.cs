using Inventory.Domain.Enum;

namespace Inventory.Domain.Entities
{
    public class AuditHistory
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid BusinessId { get; set; }
        public Business Business { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public EnumAction Action { get; set; } = EnumAction.Create;
        public EnumEntity Entity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
