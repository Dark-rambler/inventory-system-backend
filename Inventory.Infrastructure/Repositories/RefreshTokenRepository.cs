using Inventory.Application.Common.Abstracts;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class RefreshTokenRepository(InventoryDbContext context) : IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenAsync(string token) =>
            await context.RefreshTokens
                .Include(rt => rt.User)
                    .ThenInclude(u => u.Role)
                .Include(rt => rt.User)
                    .ThenInclude(u => u.Business)
                .FirstOrDefaultAsync(rt => rt.Token == token);

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();
        }

        public async Task RevokeAsync(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            await context.SaveChangesAsync();
        }
    }
}
