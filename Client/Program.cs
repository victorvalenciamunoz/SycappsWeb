using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SycappsWeb.Client;
using SycappsWeb.Client.Services;
using SycappsWeb.Client.Services.Un2Trek;
using Syncfusion.Blazor;


Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjU1NTA4NUAzMjMyMmUzMDJlMzBlckRqQ1RUOTdvM1pGYmZtYmJrWDRJSUhXa3ZrSzhCdjZmMFBNMWdyN1VNPQ==");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7034/api/") });
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPoiService, PoiService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IActividadService, ActividadService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomStateProvider>();

builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();


