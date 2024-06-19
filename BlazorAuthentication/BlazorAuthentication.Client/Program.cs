
using BitzArt.Blazor.Cookies;
using BlazorAuthentication.Client.Model;
using BlazorAuthentication.Client.Service.Authentication;
using BlazorAuthentication.Client.Service.Interface;
using BlazorAuthentication.Client.Service.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProviderClient>();
var apiMobilizeIOTInventario = builder.Configuration["ApiMobilizeIOTInventario"];
var apiMobilizeOauth = builder.Configuration["ApiMobilizeOauth"];
builder.Services.AddHttpClient("ApiMobilizeOauth", options =>
{
    options.BaseAddress = new Uri(apiMobilizeOauth);
});
builder.Services.AddHttpClient("ApiMobilizeIOTInventario", client =>
{
    client.BaseAddress = new Uri(apiMobilizeIOTInventario);
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAuthorizationCore();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXlfeHVVRmBZVERwXko=");
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ThemeState>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IFilialService, FilialService>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddRadzenComponents();
builder.Services.AddCascadingAuthenticationState();

builder.AddBlazorCookies();
await builder.Build().RunAsync();
