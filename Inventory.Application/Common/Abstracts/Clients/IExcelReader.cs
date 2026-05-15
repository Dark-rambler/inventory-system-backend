using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts.Clients
{
    public interface IExcelReader
    {
        Task<List<Product>> ReadProductsAsync(Stream stream);
        Stream GenerateProductsTemplate();
    }
}