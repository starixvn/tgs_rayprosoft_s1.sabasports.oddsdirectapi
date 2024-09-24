using MediatR;
using S1.SabaSports.OddsDirectAPI.Models;
using S1.SabaSports.OddsDirectAPI.Requests;
using S1.SabaSports.OddsDirectAPI.Services;

namespace S1.SabaSports.OddsDirectAPI.Handlers;

public class SabaHandler(ISabaService sabaService, IApiClient apiClient) : IRequestHandler<GetHighLightMatchRequest, Matches>
{
    public async Task<Matches> Handle(GetHighLightMatchRequest request, CancellationToken cancellationToken)
    {
        var token = await sabaService.LoginSabaAsync(request.BrandId);

        var results = await sabaService.GetHighLightMatchAsync(token);

        return results;
    }
}
