namespace Inventory.Application.DataTransferObjects.BranchDto
{
    public class BranchResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
