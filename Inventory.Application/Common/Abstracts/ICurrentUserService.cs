namespace Inventory.Application.Common.Abstracts
{
    public interface ICurrentUserService
    {
        Guid GetCurrentUserId();
    }
}
