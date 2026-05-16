using Inventory.Application.Common.Abstracts;
using Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class BusinessSaleCounterRepository(InventoryDbContext context) : IBusinessSaleCounterRepository
    {
        public async Task<string> GetNextFolioAsync(Guid businessId)
        {
            var counter = context.Database
                .SqlQuery<int>(
                    $"""
                     INSERT INTO "BusinessSaleCounters" ("Id", "Counter")
                     VALUES ({businessId}, 1)
                     ON CONFLICT ("Id") DO UPDATE
                         SET "Counter" = "BusinessSaleCounters"."Counter" + 1
                     RETURNING "Counter"
                     """)
                .AsEnumerable()
                .First();

            return $"POS-{counter:D4}";
        }
    }
}
