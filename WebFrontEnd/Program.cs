
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
using WebFrontEnd.Services.BusinessPartners;
using WebFrontEnd.Services.CreditNotes;
using WebFrontEnd.Services.Dashboard;
using WebFrontEnd.Services.DeliveryNotes;
using WebFrontEnd.Services.Help;
using WebFrontEnd.Services.IncomingPayments;

using WebFrontEnd.Services.Invoices;
using WebFrontEnd.Services.ItemExtension;
using WebFrontEnd.Services.ItemGroups;
using WebFrontEnd.Services.Items;
using WebFrontEnd.Services.LicenseService;
using WebFrontEnd.Services.LicenseService.Dto;
using WebFrontEnd.Services.Orders;
using WebFrontEnd.Services.OrdersCarts;
using WebFrontEnd.Services.Quotations;
using WebFrontEnd.Services.Returns;
using WebFrontEnd.Services.SQLQueries;
using WebFrontEnd.Services.Toast;
using WebFrontEnd.Services.VatGroups;
using WebFrontEnd.Services.Warehouses;
using WebFrontEnd.Shared.Dtos;
using static System.Net.WebRequestMethods;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.Configure<LicenseSettings>(
    builder.Configuration.GetSection("License"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


builder.Services.AddScoped<MessageDialogService>();
builder.Services.AddScoped<IOrderService, OrderServices>();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<IVatGroupsService, VatGroupsService>();
builder.Services.AddScoped<IBusinessPartnersService, BusinessPartnersService>();
builder.Services.AddScoped<IWarehousesService, WarehousesService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IInvoicesService, InvoicesService>();
builder.Services.AddScoped<IIncomingPaymentsService, IncomingPaymentsService>();
builder.Services.AddScoped<IQuotationsService, QuotationsService>();
builder.Services.AddScoped<IDeliveryNotesService, DeliveryNotesService>();
builder.Services.AddScoped<ICreditNotesService, CreditNotesService>();
builder.Services.AddScoped<IItemGroupsService, ItemGroupsService>();
builder.Services.AddScoped<IOrdersCartService, OrdersCartService>();
builder.Services.AddScoped<IItemExtensionService, ItemExtensionService>();
builder.Services.AddScoped<ISQLQueriesService, SQLQueriesService>();
builder.Services.AddScoped<IAppSettingsService, AppSettingsService>();
builder.Services.AddScoped<ILicenseKeyService, LicenseKeyService>();
builder.Services.AddScoped<IReturnsService, ReturnsService>();

builder.Services.AddScoped<OrdersCartService>();
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
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
})
.AddHttpMessageHandler<AuthHandler>();

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();
