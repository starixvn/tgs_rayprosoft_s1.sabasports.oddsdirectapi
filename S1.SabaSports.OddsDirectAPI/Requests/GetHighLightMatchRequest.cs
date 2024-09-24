using MediatR;
using S1.SabaSports.OddsDirectAPI.Models;

namespace S1.SabaSports.OddsDirectAPI.Requests;

public class GetHighLightMatchRequest : IRequest<IEnumerable<Match>>
{
    public Guid BrandId { get; set; }

    public int Bettype { get; set; }

    public int Top { get; set; }
}
