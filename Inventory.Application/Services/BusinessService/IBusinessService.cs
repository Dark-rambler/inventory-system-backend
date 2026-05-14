using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BusinessDto;

namespace Inventory.Application.Services.BusinessService
{
    public interface IBusinessService
    {
        Task<BusinessResponse> CreateBusinessAsync(BusinessRequest request);
        Task<PaginatedList<BusinessResponse>> GetBusinessesAsync(BusinessSearchParams searchParams);
    }
}
