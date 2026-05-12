namespace Inventory.Application.Common.Abstracts
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
