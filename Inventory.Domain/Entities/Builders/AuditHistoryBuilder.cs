using Inventory.Domain.Enum;

namespace Inventory.Domain.Entities
{
    public class AuditHistoryBuilder
    {
        private readonly AuditHistory _auditHistory = new();

        public AuditHistoryBuilder WithId(int id)
        {
            _auditHistory.Id = id;
            return this;
        }

        public AuditHistoryBuilder WithDescription(string description)
        {
            _auditHistory.Description = description;
            return this;
        }

        public AuditHistoryBuilder WithUserId(Guid userId)
        {
            _auditHistory.UserId = userId;
            return this;
        }

        public AuditHistoryBuilder WithUser(User user)
        {
            _auditHistory.User = user;
            return this;
        }

        public AuditHistoryBuilder WithAction(EnumAction action)
        {
            _auditHistory.Action = action;
            return this;
        }

        public AuditHistoryBuilder WithEntity(EnumEntity entity)
        {
            _auditHistory.Entity = entity;
            return this;
        }

        public AuditHistoryBuilder WithCreatedAt(DateTime createdAt)
        {
            _auditHistory.CreatedAt = createdAt;
            return this;
        }

        public AuditHistory Build() => _auditHistory;
    }
}