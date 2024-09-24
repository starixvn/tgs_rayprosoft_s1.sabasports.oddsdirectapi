using MediatR;
using Microsoft.AspNetCore.Mvc;
using S1.SabaSports.OddsDirectAPI.Extensions;
using S1.SabaSports.OddsDirectAPI.Requests;

namespace S1.SabaSports.OddsDirectAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SabaController(IMediator mediator) : ControllerBase
{

    [HttpGet("event")]
    public async Task<IActionResult> Get()
    {
        var brandId = HttpContext.Items.GetBrandId();
        var request = new GetHighLightMatchRequest { BrandId = brandId };
        return Ok(await mediator.Send(request).ConfigureAwait(false));
    }
}
