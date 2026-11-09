using System.Threading.RateLimiting;
using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Persistence.Extensions;
using Microsoft.AspNetCore.Authentication;
using WebAPI.Authentication;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "InternalApiKey";
        options.DefaultChallengeScheme = "InternalApiKey";
    })
    .AddScheme<AuthenticationSchemeOptions,
        ApiKeyAuthenticationHandler>(
            "InternalApiKey",
            options => { });

builder.Services.AddAuthorization();

// Add Rate Limiter
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});

builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureApplicationApp();


 

builder.Services.AddHttpClient<ILeadApiClient, LeadApiClient>(
    (serviceProvider, client) =>
    {
        var configuration = serviceProvider
            .GetRequiredService<IConfiguration>();

        var baseUrl = configuration["LicensingApi:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl) ||
            !Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp &&
             uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException(
                "LicensingApi:BaseUrl não está configurada corretamente.");
        }

        client.BaseAddress = uri;

        // Timeout de 3 minutos
        client.Timeout = TimeSpan.FromMinutes(3);
    });

var app = builder.Build();

await DatabaseInitializerExtension.InitializeDatabaseAsync(
    app.Services,
    builder.Configuration);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRateLimiter();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


app.Run();


