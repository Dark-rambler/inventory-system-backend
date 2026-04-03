using Inventory.Application.DataTransferObjects.Location;

namespace Inventory.Application.DataTransferObjects.BranchDto
{
    public class BranchRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public LocationRequest Location { get; set; } = default!;
    }
}
