namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public class SaleSearchParams
    {
        public string? Branch { get; set; }
        public string? Seller { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
