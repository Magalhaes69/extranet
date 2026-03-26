using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebFrontEnd;
using WebFrontEnd.Services;
using WebFrontEnd.Services.Auth;
using WebFrontEnd.Services.BusinessPartners;
using WebFrontEnd.Services.Dashboard;
using WebFrontEnd.Services.IncomingPayments;
using WebFrontEnd.Services.IncomingPayments.Dto;
using WebFrontEnd.Services.Invoices;
using WebFrontEnd.Services.Items;
using WebFrontEnd.Services.Orders;
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

builder.Services.AddScoped<AuthService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
