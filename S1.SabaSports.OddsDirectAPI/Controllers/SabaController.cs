using MediatR;
using Microsoft.AspNetCore.Mvc;
using S1.SabaSports.OddsDirectAPI.Attributes;
using S1.SabaSports.OddsDirectAPI.Extensions;
using S1.SabaSports.OddsDirectAPI.Requests;

namespace S1.SabaSports.OddsDirectAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[S1ResponseWrapper]
public class SabaController(IMediator mediator) : ControllerBase
{

    [HttpGet("event")]
    public async Task<IActionResult> Get(int betType, int top)
    {
        var brandId = HttpContext.Items.GetBrandId();
        var request = new GetHighLightMatchRequest { BrandId = brandId, Bettype = betType, Top = top };
        return Ok(await mediator.Send(request).ConfigureAwait(false));
    }
}
