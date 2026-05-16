namespace Inventory.Application.Common.Abstracts
{
    public interface IBusinessSaleCounterRepository
    {
        Task<string> GetNextFolioAsync(Guid businessId);
    }
}
