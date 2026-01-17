using Inventory.Application.DataTransferObjects.BranchDto;

namespace Inventory.Application.DataTransferObjects.WarehouseDto
{
    public class WarehouseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public BranchResponse Branch { get; set; } = default!;
    }
}
