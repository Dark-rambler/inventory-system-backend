using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.AuditHistoryDto;
using Inventory.Application.Services.AuditHistoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuditHistoryController(IAuditHistoryService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<AuditHistoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditHistoriesAsync([FromQuery] AuditHistorySearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetAuditHistoriesAsync(searchParams, businessId));
        }
    }
}