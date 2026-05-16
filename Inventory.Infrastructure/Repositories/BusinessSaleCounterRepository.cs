using Inventory.Application.Common.Abstracts;
using Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class BusinessSaleCounterRepository(InventoryDbContext context) : IBusinessSaleCounterRepository
    {
        public async Task<string> GetNextFolioAsync(Guid businessId)
        {
            var results = await context.Database
                .SqlQuery<int>(
                    $"""
                     INSERT INTO business_sale_counters (business_id, counter)
                     VALUES ({businessId}, 1)
                     ON CONFLICT (business_id) DO UPDATE
                         SET counter = business_sale_counters.counter + 1
                     RETURNING counter
                     """)
                .ToListAsync();

            var counter = results.First();

            return $"POS-{counter:D4}";
        }
    }
}
