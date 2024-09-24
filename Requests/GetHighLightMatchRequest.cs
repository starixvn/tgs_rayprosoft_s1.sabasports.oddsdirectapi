using MediatR;
using S1.SabaSports.OddsDirectAPI.Models;

namespace S1.SabaSports.OddsDirectAPI.Requests;

public class GetHighLightMatchRequest : IRequest<Matches>
{
    public Guid BrandId { get; set; }
}
