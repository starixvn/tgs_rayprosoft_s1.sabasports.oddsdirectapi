namespace TGS.Apis.OperatorApi.Middlewares;

public sealed class ReadBrandIdMiddleware(
    RequestDelegate next)
{
    private const string brandIdKey = "b-id";
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            if (context.Request.Headers.ContainsKey(brandIdKey))
            {
                var brandId = context.Request.Headers[brandIdKey];
                context.Items.Add("BrandId", brandId);
            }
        }
        finally
        {
            await _next(context).ConfigureAwait(false);
        }
    }
}