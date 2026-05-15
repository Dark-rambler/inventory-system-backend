using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IMeasureRepository
    {
        Task<List<Measure>> GetMeasuresAsync();
    }
}