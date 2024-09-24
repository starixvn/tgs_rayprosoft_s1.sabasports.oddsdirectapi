using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using S1.SabaSports.OddsDirectAPI.Models;
using S1.SabaSports.OddsDirectAPI.Options;
using S1.SabaSports.OddsDirectAPI.Requests;

namespace S1.SabaSports.OddsDirectAPI.Services;


public interface ISabaService
{
    Task<TokenInfo> LoginSabaAsync(Guid brandId);

    Task<IEnumerable<Match>> GetHighLightMatchAsync(GetHighLightMatchRequest request, TokenInfo token);
}
public class SabaService(IOptions<SabaSettings> options, IMemoryCache memoryCache, IApiClient apiClient) : ISabaService
{
    public async Task<TokenInfo> LoginSabaAsync(Guid brandId)
    {
        var token = await memoryCache.GetOrCreateAsync(brandId, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(options.Value.TokenExpiration);

            return await apiClient.PostDataAsync<TokenInfo>($"{options.Value.Url}/login", new
            {
                Vendor_Id = options.Value.VendorId,
                Vendor_member_id = options.Value.VendorMemberId
            });
        });

        return token ?? throw new AuthenticationFailureException("Login authentication failed");
    }

    public async Task<IEnumerable<Match>> GetHighLightMatchAsync(GetHighLightMatchRequest request, TokenInfo token)
    {
        var eventMarket = await GetEventAsync(request, token);

        return eventMarket.Events.Select(e => new Match
        {
            SportType = e.SportType,
            SportName = e.SportName,
            LeagueId = e.LeagueId,
            LeagueName = e.LeagueName,
            LeagueIconUrl = e.LeagueIconUrl,
            EventId = e.EventId,
            EventCode = e.EventCode,
            EventStatus = e.EventStatus,
            IsMainMarket = e.IsMainMarket,
            KickOffTime = e.KickOffTime,
            GlobalShowTime = e.GlobalShowTime,
            CountryCode = e.CountryCode,
            IsLive = e.IsLive,
            MarketCount = e.MarketCount,
            ChannelCode = e.ChannelCode,
            TeamInfo = e.TeamInfo,
            GameInfo = e.GameInfo,
            Markets = eventMarket.Markets.Where(m => m.EventId == e.EventId)
        }).Where(m => m.Markets != null && m.Markets.Any()); ;
    }

    private async Task<EventMarket> GetEventAsync(GetHighLightMatchRequest request, TokenInfo token)
    {
        return await apiClient.GetDataAsync<EventMarket>(
            $"{options.Value.Url}/sports/v1/GetEvents?query=$filter=sporttype eq 1&$top={request.Top}&includeMarkets=$filter=bettype eq {request.Bettype}&language=en",
            token.Access_Token);
    }
}
