using Inventory.Application.Services.ProductService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService service ) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            return Ok(await service.GetProductByIdAsync(id));
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = new[]
            {
                new { Id = 1, Name = "Sample Product 1", Price = 9.99 },
                new { Id = 2, Name = "Sample Product 2", Price = 19.99 }
            };
            return Ok(products);
        }
    }
}
