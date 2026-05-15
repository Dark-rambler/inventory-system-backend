using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BusinessDto;
using Inventory.Application.Services.BusinessService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BusinessController(IBusinessService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<BusinessResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBusinessesAsync([FromQuery] BusinessSearchParams searchParams) =>
            Ok(await service.GetBusinessesAsync(searchParams));

        [HttpPost]
        [ProducesResponseType(typeof(BusinessResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBusinessAsync([FromBody] BusinessRequest request) =>
            Ok(await service.CreateBusinessAsync(request));
    }
}
