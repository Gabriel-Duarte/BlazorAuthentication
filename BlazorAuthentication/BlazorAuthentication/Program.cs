using BitzArt.Blazor.Cookies;
using BlazorAuthentication.Client.Model;
using BlazorAuthentication.Client.Pages;
using BlazorAuthentication.Client.Service.Authentication;
using BlazorAuthentication.Client.Service.Interface;
using BlazorAuthentication.Client.Service.Service;
using BlazorAuthentication.Components;
using BlazorAuthentication.Service.Authentication;
using BlazorAuthentication.Service.Interfaces;
using BlazorAuthentication.Service.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor(); // Adiciona HttpContextAccessor
builder.Services.AddScoped<AuthenticationStateProvider,ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorizationCore();
builder.AddBlazorCookies();
builder.Services.AddScoped<ThemeState>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IFilialService, FilialService>();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXlfeHVVRmBZVERwXko=");
builder.Services.AddSyncfusionBlazor();
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

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddRadzenComponents();
//builder.Services.AddRazorComponents()
//    .AddInteractiveServerComponents()
//    .AddInteractiveWebAssemblyComponents();

//builder.Services.AddRazorComponents();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.MapRazorComponents<App>()
//    .AddInteractiveServerRenderMode()
//     .AddInteractiveWebAssemblyRenderMode();


app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorAuthentication.Client._Imports).Assembly);

app.Run();
