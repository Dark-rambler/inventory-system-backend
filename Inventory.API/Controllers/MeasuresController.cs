using Inventory.Application.DataTransferObjects.MeasureDto;
using Inventory.Application.Services.MeasureService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MeasuresController(IMeasureService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<MeasureResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeasuresAsync()
        {
            return Ok(await service.GetMeasuresAsync());
        }
    }
}