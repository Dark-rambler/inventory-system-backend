using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProviderDto;
using Inventory.Application.Services.ProviderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProviderController(IProviderService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaginatedList<ProviderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProvidersAsync([FromQuery] ProviderSearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetProvidersAsync(searchParams, businessId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProviderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProviderByIdAsync(Guid id)
        {
            return Ok(await service.GetProviderByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProviderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProviderAsync([FromBody] ProviderRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.CreateProviderAsync(request, businessId));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateProviderAsync(Guid id, [FromBody] ProviderRequest request)
        {
            await service.UpdateProviderAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProviderAsync(Guid id)
        {
            await service.DeleteProviderAsync(id);
            return NoContent();
        }
    }
}