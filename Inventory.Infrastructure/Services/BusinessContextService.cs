using Inventory.Application.Common.Abstracts;
using Microsoft.AspNetCore.Http;

namespace Inventory.Infrastructure.Services;

public class BusinessContextService(IHttpContextAccessor httpContextAccessor) : IBusinessContextService
{
    public Guid GetBusinessId()
    {
        var header = httpContextAccessor.HttpContext?.Request.Headers["businessId"].FirstOrDefault()
            ?? throw new UnauthorizedAccessException("Business ID header is missing.");
        return Guid.TryParse(header, out var id)
            ? id
            : throw new ArgumentException("Business ID header is invalid.");
    }
}
