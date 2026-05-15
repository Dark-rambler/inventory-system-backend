using Inventory.Application.Common.Abstracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Inventory.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public Guid GetCurrentUserId()
        {
            var claim = httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User is not authenticated.");
            return Guid.Parse(claim.Value);
        }
    }
}
