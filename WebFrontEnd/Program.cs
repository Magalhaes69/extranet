using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using WebFrontEnd;
using WebFrontEnd.Extensions;
using WebFrontEnd.Helps;
using WebFrontEnd.Services;
using WebFrontEnd.Services.Auth;
using WebFrontEnd.Services.BusinessPartners;
using WebFrontEnd.Services.CreditNotes;
using WebFrontEnd.Services.Dashboard;
using WebFrontEnd.Services.DeliveryNotes;
using WebFrontEnd.Services.IncomingPayments;

using WebFrontEnd.Services.Invoices;
using WebFrontEnd.Services.Items;
using WebFrontEnd.Services.Orders;
using WebFrontEnd.Services.Quotations;
using WebFrontEnd.Services.Toast;
using WebFrontEnd.Services.VatGroups;
using WebFrontEnd.Services.Warehouses;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

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
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});


builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<UiConverters>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();


var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();
