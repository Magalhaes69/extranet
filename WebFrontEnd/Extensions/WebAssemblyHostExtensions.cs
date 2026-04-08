using System.Globalization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using WebFrontEnd.Services.CultureServices;

namespace WebFrontEnd.Extensions
{
    public static class WebAssemblyHostExtensions
    {
        public async static Task SetDefaultCulture(this WebAssemblyHost host)
        {
           
            var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
            var cultureName = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "blazorCulture.get"); 

            CultureInfo cultureInfo;
            if (!string.IsNullOrWhiteSpace(cultureName))
            {
                cultureInfo = new CultureInfo(cultureName);
            }
            else
            {
                cultureInfo = new CultureInfo(LocalizerSettings.NeutralCulture.Name);
            }

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        }
    }
}
