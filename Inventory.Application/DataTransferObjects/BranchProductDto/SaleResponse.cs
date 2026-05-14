namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public class SaleResponse
    {
        public Guid Id { get; set; }
        public string Folio { get; set; } = string.Empty;
        public double Total { get; set; }
        public string Branch { get; set; } = string.Empty;
        public string Seller { get; set; } = string.Empty;
        public string? Customer { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<SaleDetailResponse> SaleDetails { get; set; } = [];
    }
}
