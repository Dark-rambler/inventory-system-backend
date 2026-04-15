using Inventory.Domain.Enum;

namespace Inventory.Application.DataTransferObjects.AuditHistoryDto
{
    public class AuditHistorySearchParams
    {
        public Guid? User { get; set; }
        public EnumAction? Action { get; set; }
        public EnumEntity? Entity { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}