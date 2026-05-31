using Inventory.Application.DataTransferObjects.DashboardDto;
using Inventory.Application.Services.DashboardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController(IDashboardService service) : ControllerBase
    {
        [HttpGet("today")]
        [ProducesResponseType(typeof(DashboardResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTodayStatsAsync(
            [FromQuery] Guid? branchId,
            [FromHeader][BindRequired] Guid businessId)
        {
            if (!User.IsInRole("Admin") && !branchId.HasValue)
                return BadRequest("branchId is required for non-admin users.");
            return Ok(await service.GetTodayStatsAsync(businessId, branchId));
        }
    }
}
