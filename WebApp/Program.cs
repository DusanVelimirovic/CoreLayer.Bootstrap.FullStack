using Blazored.Toast;
using CoreLayer.Shared.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Net;
using WebApp.Components;
using WebApp.Helpers;
using WebApp.Services.Identity.Admin;
using WebApp.Services.Identity.Auth;


var builder = WebApplication.CreateBuilder(args);

// Configure Razor components with interactive rendering
builder.Services.AddRazorComponents() // This registers support for Razor components
    .AddInteractiveServerComponents(); // Enables interactivity via Blazor Server rendering

// Register Toast notification
builder.Services.AddBlazoredToast();

// Register Blazor Bootstrap
builder.Services.AddBlazorBootstrap();

// Register Protected Session Storage service to securely store data in the browser's session
builder.Services.AddScoped<ProtectedSessionStorage>();

// Register a cookie container
var cookieContainer = new CookieContainer();
builder.Services.AddSingleton(cookieContainer);

builder.Services.AddHttpClient("BackendAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7267/");
})
.ConfigurePrimaryHttpMessageHandler(provider =>
    new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = provider.GetRequiredService<CookieContainer>(),
        UseDefaultCredentials = true// always reuse
    });

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("BackendAPI");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
    return client;
});

// Services repository
// Auth Service & CookieRestorer

builder.Services.AddScoped<ModuleAccessHelper>(provider =>
{
    var clientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = clientFactory.CreateClient("BackendAPI");

    var cookieContainer = provider.GetRequiredService<CookieContainer>();
    var authProvider = provider.GetRequiredService<CustomAuthStateProvider>();
    var sessionStorage = provider.GetRequiredService<ProtectedSessionStorage>();
    var config = provider.GetRequiredService<IConfiguration>();
    var jsRuntime = provider.GetRequiredService<IJSRuntime>();

    return new ModuleAccessHelper(authProvider, httpClient, sessionStorage, cookieContainer, config, jsRuntime); // FIXED order
});

builder.Services.AddScoped<RoleService>(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("BackendAPI");
    return new RoleService(client);
});

builder.Services.AddScoped<ModuleService>(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("BackendAPI");
    return new ModuleService(client);
});

builder.Services.AddScoped<AccessControlService>(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("BackendAPI");
    return new AccessControlService(client);
});

builder.Services.AddScoped<AdminUserService>(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("BackendAPI");
    return new AdminUserService(client);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthService>(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient("BackendAPI");

    var jsRuntime = provider.GetRequiredService<IJSRuntime>();
    var accessor = provider.GetRequiredService<IHttpContextAccessor>();
    var sessionStorage = provider.GetRequiredService<ProtectedSessionStorage>();
    var cookieContainer = provider.GetRequiredService<CookieContainer>();
    var authStateProvider = provider.GetRequiredService<CustomAuthStateProvider>();

    return new AuthService(client, jsRuntime, accessor, sessionStorage, cookieContainer, authStateProvider);
});

// Auth State Provider
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());


// Helpers
builder.Services.AddScoped<SessionStorageHelper>();

// Register CoreLayer Auth Module
//builder.Services.AddCoreLayerAuth(builder.Configuration);




// Register cookie authentication

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection(); 
app.UseStaticFiles();
app.UseRouting();


//app.UseAuthentication(); // must come BEFORE authorization
//app.UseAuthorization();
app.UseAntiforgery();

// Remove app.MapBlazorHub();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Only this

app.Run();
