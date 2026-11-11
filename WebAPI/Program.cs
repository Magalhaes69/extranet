using System.Threading.RateLimiting;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Persistence.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using WebAPI.Authentication;
using WebAPI.Controllers;
using WebAPI.Settings;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS para permitir pedidos do frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7083", "http://localhost:7083")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

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

// Consulta das licenças do cliente no LicenseKey. Mesma API e mesma secção de
// configuração do ILeadApiClient: o cliente usa URLs relativos (api/client/licenses/{cardCode})
// e envia a LicensingApi:ApiKey no header X-Api-Key.
builder.Services.AddHttpClient<IClientLicenseApiClient, ClientLicenseApiClient>(
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

// Registo público de contas (LeadsController): secção "LeadsApi" do appsettings
// com o endereço do LicenseKey, o caminho do endpoint, a chave e as regras do trial.
builder.Services.Configure<LeadsApiSettings>(
    builder.Configuration.GetSection("LeadsApi"));

// Cliente que o LeadsController usa para reencaminhar o registo. O CreatePath é
// relativo, por isso o BaseAddress tem de ficar definido aqui.
builder.Services.AddHttpClient(
    LeadsController.HttpClientName,
    (serviceProvider, client) =>
    {
        var settings = serviceProvider
            .GetRequiredService<IOptions<LeadsApiSettings>>()
            .Value;

        if (!string.IsNullOrWhiteSpace(settings.BaseUrl) &&
            Uri.TryCreate(settings.BaseUrl, UriKind.Absolute, out var uri) &&
            (uri.Scheme == Uri.UriSchemeHttp ||
             uri.Scheme == Uri.UriSchemeHttps))
        {
            client.BaseAddress = uri;
        }

        // O controller trata a BaseUrl em falta devolvendo 503, por isso aqui não se lança:
        // uma excepção na configuração do cliente rebentaria o pedido em vez de responder.

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
app.UseCors("AllowFrontend");
app.UseRateLimiter();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


app.Run();


