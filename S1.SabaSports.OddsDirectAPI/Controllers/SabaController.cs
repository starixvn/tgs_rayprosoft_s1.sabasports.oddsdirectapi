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
    public async Task<IActionResult> Get(int sportType, int betType, int top, string from, string to)
    {
        var brandId = HttpContext.Items.GetBrandId();
        var request = new GetHighLightMatchRequest { SportType = sportType, BrandId = brandId, Bettype = betType, Top = top, From = from, To = to };
        return Ok(await mediator.Send(request).ConfigureAwait(false));
    }
}
