using S1.SabaSports.OddsDirectAPI.Controllers;
using S1.SabaSports.OddsDirectAPI.Options;
using S1.SabaSports.OddsDirectAPI.Services;
using TGS.Apis.OperatorApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json",
    optional: true,
    reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SabaController).Assembly));
builder.Services.Configure<SabaSettings>(builder.Configuration.GetSection("SABA"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IApiClient, ApiClient>();
builder.Services.AddSingleton<ISabaService, SabaService>();
builder.Services.AddMemoryCache();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SabaController).Assembly));
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/health");

app.UseMiddleware<ReadBrandIdMiddleware>();

app.MapControllers();

app.Run();
