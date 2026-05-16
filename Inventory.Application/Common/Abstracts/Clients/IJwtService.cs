using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts.Clients
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
    }
}
