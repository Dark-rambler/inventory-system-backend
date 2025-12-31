using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = new { Id = id, Name = "Sample Product", Price = 9.99 };
            return Ok(product);
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
