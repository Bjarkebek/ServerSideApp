using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerSideApp.Components;
using ServerSideApp.Components.Account;
using ServerSideApp.Data;
using ServerSideApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<HashingService>(); // tilf�jer hashing service
builder.Services.AddSingleton<SymmetricEncryptionService>(); // tilf�jer symmetrisk enkrypt service
builder.Services.AddSingleton<AsymmetricEncryptionService>(); // tilf�jer asymmetrisk enkrypt service

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// Connectionstring til ToDo database
var toDoConnectionString = builder.Configuration.GetConnectionString("ToDoConnection") ?? throw new InvalidOperationException("Connection string 'ToDoConnection' not found.");
builder.Services.AddDbContext<ServerSideApp.Models.ToDoDbContext>(options =>
    options.UseSqlServer(toDoConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


// Configure Certificate path
string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
userFolder = Path.Combine(userFolder, ".aspnet");
userFolder = Path.Combine(userFolder, "https");
userFolder = Path.Combine(userFolder, "bb.pfx");
builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate:Path").Value = userFolder;

// Get password from secrets.json
string kestrelCertPassword = builder.Configuration.GetValue<string>("KestrelCertPassword");
builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate:Password").Value = kestrelCertPassword;


// tilf�jer authorization service og policy
builder.Services.AddAuthorization(options => {
    options.AddPolicy("AuthenticatedUser", policy => {
        policy.RequireAuthenticatedUser();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
