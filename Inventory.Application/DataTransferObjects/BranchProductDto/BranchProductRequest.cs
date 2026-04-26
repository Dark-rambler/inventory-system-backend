namespace Inventory.Application.DataTransferObjects.BranchProductDto
{
    public class BranchProductRequest
    {
        public Guid ProductId { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int LowStock { get; set; }
    }
}
