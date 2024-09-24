using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using S1.SabaSports.OddsDirectAPI.Models;
using S1.SabaSports.OddsDirectAPI.Options;

namespace S1.SabaSports.OddsDirectAPI.Services;


public interface ISabaService
{
    Task<TokenInfo> LoginSabaAsync(Guid brandId);

    Task<Matches> GetHighLightMatchAsync(TokenInfo token);
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

    public async Task<Matches> GetHighLightMatchAsync(TokenInfo token)
    {
        return await apiClient.GetDataAsync<Matches>(
            $"{options.Value.Url}/sports/v1/GetEvents?query=$filter=sporttype eq 1&$top=10&includeMarkets=$filter=bettype eq 5&language=en", token.Access_Token);

    }
}
