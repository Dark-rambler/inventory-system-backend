using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task CreateAsync(RefreshToken refreshToken);
        Task RevokeAsync(RefreshToken refreshToken);
    }
}
