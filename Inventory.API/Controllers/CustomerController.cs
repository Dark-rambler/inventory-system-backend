using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CustomerDto;
using Inventory.Application.Services.CustomerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController(ICustomerService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<CustomerResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomerSearchParams searchParams)
        {
            return Ok(await service.GetCustomersAsync(searchParams));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerRequest request)
        {
            return Ok(await service.CreateCustomerAsync(request));
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCustomerAsync(Guid id, [FromBody] CustomerRequest request) {

            return Ok(await service.UpdateCustomerAsync(id, request));

        }

    }
}