namespace Inventory.Application.DataTransferObjects.PurchaseDto
{
    public class PurchaseResponse
    {
        public Guid Id { get; set; }
        public double Total { get; set; }
        public string Provider { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string Buyer { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public IEnumerable<PurchaseDetailResponse> PurchaseDetails { get; set; } = [];
    }
}