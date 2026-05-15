namespace Inventory.Application.DataTransferObjects.MeasureDto
{
    public class MeasureResponse(int Id, string Name)
    {
        public int Id { get; set; } = Id;
        public string Name { get; set; } = Name;
    }
}