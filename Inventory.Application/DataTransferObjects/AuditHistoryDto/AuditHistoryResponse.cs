namespace Inventory.Application.DataTransferObjects.AuditHistoryDto
{
    public class AuditHistoryResponse
    {
        public int Id { get; set; }
        public string User { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}