using Inventory.Domain.Enum;
using System.Text.Json.Serialization;

namespace Inventory.Application.DataTransferObjects.InventoryMovementDto
{
    public class InventoryMovementResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnumMovementType Type { get; set; }
        public string FromWarehouse { get; set; } = string.Empty;
        public string FromBranch { get; set; } = string.Empty;
        public string ToWarehouse { get; set; } = string.Empty;
        public string ToBranch { get; set; } = string.Empty;
    }
}
