using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.PurchaseDto;
using Inventory.Application.Services.PurchaseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseController(IPurchaseService service) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreatePurchaseAsync([FromBody] PurchaseRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            await service.CreatePurchaseAsync(request, businessId);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<PurchaseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPurchasesAsync([FromQuery] PurchaseSearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetPurchasesAsync(searchParams, businessId));
        }
    }
}
