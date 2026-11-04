using System.Threading.RateLimiting;
using Application.Services;
using Microsoft.Extensions.Options;
using Persistence.Extensions;
using WebAPI.Controllers;
using WebAPI.Settings;
using WebFrontEnd.Services.Licenses;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureApplicationApp();

// "Create account": the public form posts to our LeadsController, which forwards to the leads API
builder.Services.Configure<LeadsApiSettings>(builder.Configuration.GetSection("LeadsApi"));
builder.Services.AddHttpClient(LeadsController.HttpClientName, (sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<LeadsApiSettings>>().Value;
    if (!string.IsNullOrWhiteSpace(settings.BaseUrl))
        client.BaseAddress = new Uri(settings.BaseUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(15);
});

// The endpoint is anonymous, so it is limited per IP address
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy(LeadsController.RateLimitPolicy, httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(10),
                QueueLimit = 0
            }));
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(
            "{\"flag\":false,\"message\":\"Demasiados pedidos. Tente novamente dentro de alguns minutos.\"}",
            cancellationToken);
    };
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
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


app.Run();
