using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using WebFrontEnd;
using WebFrontEnd.Extensions;
using WebFrontEnd.Helps;
using WebFrontEnd.Services;
using WebFrontEnd.Services.AppSettings;
using WebFrontEnd.Services.Auth;
using WebFrontEnd.Services.Dashboard;
using WebFrontEnd.Services.Help;
using WebFrontEnd.Services.LicenseService;
using WebFrontEnd.Services.LicenseService.Dto;
using WebFrontEnd.Services.Requests;
using WebFrontEnd.Services.Toast;
using WebFrontEnd.Services.Users;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.Configure<LicenseSettings>(
    builder.Configuration.GetSection("License"));

// Ler a BaseUrl da API a partir do appsettings.json
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });


// Serviços essenciais
builder.Services.AddScoped<MessageDialogService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAppSettingsService, AppSettingsService>();
builder.Services.AddScoped<ILicenseKeyService, LicenseKeyService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IRequestsService, RequestsService>();
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services.Configure<LicenseSettings>(
    builder.Configuration.GetSection("License"));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<UiConverters>();
builder.Services.AddScoped<PageStateService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<NavigationHistoryService>();


builder.Services.AddScoped<AuthHandler>();
builder.Services.AddScoped<DynamicFilterBlazorHelper>();
builder.Services.AddScoped<FilterMetadataBuilder>();
builder.Services.AddScoped<LayoutStateService>();
builder.Services.AddScoped<PageTitleService>();
builder.Services.AddScoped<UiStateService>();
builder.Services.AddSingleton<LicenseState>();
builder.Services.AddHttpClient("API", client =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? builder.HostEnvironment.BaseAddress;
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHandler>();

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();