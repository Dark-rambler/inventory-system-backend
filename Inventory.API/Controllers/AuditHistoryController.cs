using Inventory.Application.DataTransferObjects.AuditHistoryDto;
using Inventory.Application.Services.AuditHistoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuditHistoryController(IAuditHistoryService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(AuditHistoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuditHistoriesAsync([FromQuery] AuditHistorySearchParams searchParams)
        {
            return Ok(await service.GetAuditHistoriesAsync(searchParams));
        }
    }
}