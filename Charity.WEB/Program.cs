//<!-- Author xkrukh00-->
using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Charity.WEB.BL.Extensions;
using Charity.WEB.BL.Features;
using Charity.WEB.BL.Installers;
using Microsoft.AspNetCore.Components.Authorization;
using Majorsoft.Blazor.Components.CssEvents;
using Majorsoft.Blazor.Components.Notifications;

namespace Charity.WEB
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");

            builder.Services.AddInstaller<WebBLInstaller>(apiBaseUrl);
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<AuthenticationStateProvider, CharityAuthStateProvider>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddCssEvents();
            builder.Services.AddNotifications();
            builder.Services.AddBlazoredModal();


            var host = builder.Build();
            await host.RunAsync();
        }
    }
}