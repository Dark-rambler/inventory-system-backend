using Inventory.Application.Common.Abstracts;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class BusinessSaleCounterRepository(InventoryDbContext context) : IBusinessSaleCounterRepository
    {
        public async Task<string> GetNextFolioAsync(Guid businessId)
        {
            var counterEntity = await context.BusinessSaleCounters
                .FirstOrDefaultAsync(c => c.BusinessId == businessId);
            int nextValue;
            if (counterEntity == null)
            {
                nextValue = 1;
                counterEntity = new BusinessSaleCounter
                {
                    BusinessId = businessId,
                    Counter = nextValue
                };
                context.BusinessSaleCounters.Add(counterEntity);
            }
            else
            {
                counterEntity.Counter += 1;
                nextValue = counterEntity.Counter;
            }
            await context.SaveChangesAsync();
            return $"POS-{nextValue:D4}";
        }
    }
}
