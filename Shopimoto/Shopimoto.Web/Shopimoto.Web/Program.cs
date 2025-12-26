using Shopimoto.Web.Client.Pages;
using Shopimoto.Web.Components;

using Microsoft.EntityFrameworkCore;
using Shopimoto.Infrastructure.Data;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
Env.Load();

// Configure Database Connection
var connectionString = $"Host={Env.GetString("DB_HOST", "localhost")};" +
                       $"Port={Env.GetString("DB_PORT", "5432")};" +
                       $"Database={Env.GetString("DB_NAME", "shopimoto_db")};" +
                       $"Username={Env.GetString("DB_USER", "postgres")};" +
                       $"Password={Env.GetString("DB_PASSWORD", "postgres")}";

builder.Services.AddDbContext<ShopimotoDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register Repositories and Services
builder.Services.AddScoped<Shopimoto.Domain.Interfaces.IUserRepository, Shopimoto.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Shopimoto.Application.Interfaces.IAuthService, Shopimoto.Application.Services.AuthService>();
builder.Services.AddScoped<Shopimoto.Domain.Interfaces.IProductRepository, Shopimoto.Infrastructure.Repositories.ProductRepository>();
builder.Services.AddScoped<Shopimoto.Application.Interfaces.IProductService, Shopimoto.Application.Services.ProductService>();
builder.Services.AddScoped<Shopimoto.Domain.Interfaces.ICartRepository, Shopimoto.Infrastructure.Repositories.CartRepository>();
builder.Services.AddScoped<Shopimoto.Application.Interfaces.ICartService, Shopimoto.Application.Services.CartService>();
builder.Services.AddScoped<Shopimoto.Domain.Interfaces.IOrderRepository, Shopimoto.Infrastructure.Repositories.OrderRepository>();
builder.Services.AddScoped<Shopimoto.Application.Interfaces.IOrderService, Shopimoto.Application.Services.OrderService>();
builder.Services.AddScoped<Shopimoto.Domain.Interfaces.IAddressRepository, Shopimoto.Infrastructure.Repositories.AddressRepository>();
builder.Services.AddScoped<Shopimoto.Application.Interfaces.IAddressService, Shopimoto.Application.Services.AddressService>();
builder.Services.AddScoped<Shopimoto.Domain.Interfaces.IReviewRepository, Shopimoto.Infrastructure.Repositories.ReviewRepository>();
builder.Services.AddScoped<Shopimoto.Application.Interfaces.IReviewService, Shopimoto.Application.Services.ReviewService>();
builder.Services.AddScoped<Shopimoto.Domain.Interfaces.IWishlistRepository, Shopimoto.Infrastructure.Repositories.WishlistRepository>();
builder.Services.AddScoped<Shopimoto.Application.Interfaces.IWishlistService, Shopimoto.Application.Services.WishlistService>();
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromDays(7);
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add services to the container.
builder.Services.AddScoped<Shopimoto.Infrastructure.Data.DatabaseSeeder>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

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

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Shopimoto.Web.Client._Imports).Assembly);

app.Run();
