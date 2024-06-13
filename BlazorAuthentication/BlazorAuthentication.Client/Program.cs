
using BitzArt.Blazor.Cookies;
using BlazorAuthentication.Client.Service.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProviderClient>();
builder.Services.AddHttpClient();
builder.Services.AddAuthorizationCore();
builder.AddBlazorCookies();
await builder.Build().RunAsync();
