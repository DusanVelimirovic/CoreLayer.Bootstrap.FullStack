using Backend.Data;
using Backend.Models.Identity;
using Backend.Services.Auth;
using Backend.Services.Identity;
using Backend.Services.Interfaces.Auth;
using Backend.Services.Interfaces.Email;
using Backend.Services.Interfaces.Identity;
using Backend.Services.Email;
using CoreLayer.Shared.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add CoreLayer Shared Auth Module
builder.Services.AddCoreLayerAuth(builder.Configuration);


// SendGrid configuration
builder.Services.AddSendGrid(options =>
    options.ApiKey = builder.Configuration.GetValue<string>("SendGridApiKey")
        ?? throw new Exception("The 'SendGridApiKey' is not configured")
);

// Data Protection (for persistent cookie encryption)
var dataProtectionBuilder = builder.Services.AddDataProtection()
    .SetApplicationName("CoreLayer");

if (builder.Environment.IsDevelopment())
{
    var localKeysDir = Path.Combine("C:", "CoreLayerDataProtectionKeys");
    Directory.CreateDirectory(localKeysDir); // Ensure folder exists
    dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(localKeysDir));
}
else
{
    // For Azure or production, a different location like blob storage
    var azureKeysDir = Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? "D:\\home", "data", "keys");
    Directory.CreateDirectory(azureKeysDir);
    dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(azureKeysDir));
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy to allow frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins("https://localhost:7239")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Identity setup
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;

        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<CoreLayerDbContext>()
    .AddDefaultTokenProviders();

// Token lifespan for 2FA etc.
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.Name = TokenOptions.DefaultProvider;
    options.TokenLifespan = TimeSpan.FromMinutes(10);
});

// Configure cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".AspNetCore.Identity.Application";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None; // critical for cross-origin
        options.Cookie.Path = "/";
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(365);
    });


// Services Repository
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<TwoFactorCleanupService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<IAdminRoleService, AdminRoleService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IAdminAccessControlService, AdminAccessControlService>();
builder.Services.AddHttpContextAccessor();

// Email services (env based)
if (builder.Environment.IsDevelopment())
{
    Console.WriteLine(" EmailService: Using ConsoleEmailService (Development)");
    builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
}
else
{
    Console.WriteLine(" EmailService: Using SendGridEmailService (Non-Development)");
    builder.Services.AddScoped<IEmailService, SendGridEmailService>();
}

// Register DB Context
builder.Services.AddDbContext<CoreLayerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthorization();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowWebApp"); // <-- Add CORS middleware here


// Identity middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// await SeedData.InitializeAsync(app.Services);

app.Run();
