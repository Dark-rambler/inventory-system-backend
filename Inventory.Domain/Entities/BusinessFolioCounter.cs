namespace Inventory.Domain.Entities
{
    public class BusinessFolioCounter
    {
        public Guid BusinessId { get; set; }
        public int LastFolioNumber { get; set; }
        public Business Business { get; set; } = null!;
    }
}
