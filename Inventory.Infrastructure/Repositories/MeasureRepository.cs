using Inventory.Application.Common.Abstracts;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class MeasureRepository(InventoryDbContext context) : IMeasureRepository
    {
        public async Task<List<Measure>> GetMeasuresAsync()
        {
            return await context.Measures.ToListAsync();
        }
    }
}