using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CustomerDto;
using Inventory.Application.Services.CustomerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController(ICustomerService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<CustomerResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomerSearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetCustomersAsync(searchParams, businessId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.CreateCustomerAsync(request, businessId));
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCustomerAsync(Guid id, [FromBody] CustomerRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            await service.UpdateCustomerAsync(id, request, businessId);
            return NoContent();
        }

    }
}