using Microsoft.AspNetCore.Authentication.Cookies;
using ReliZone.Web.HttpClients;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("HttpClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiGatewayAddress"]);
});

builder.Services.AddScoped(sp =>
{
    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpCleint = httpClientfactory.CreateClient("HttpClient");
    return new CatalogServiceClient(httpCleint);
});

builder.Services.AddScoped<AuthServiceClient>(sp =>
{
    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientfactory.CreateClient("HttpClient");
    return new AuthServiceClient(httpClient);
});

builder.Services.AddScoped<CartServiceClient>(sp =>
{
    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientfactory.CreateClient("HttpClient");
    return new CartServiceClient(httpClient);
});
builder.Services.AddScoped<PaymentServiceClient>(sp =>
{
    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientfactory.CreateClient("HttpClient");
    return new PaymentServiceClient(httpClient);
});
builder.Services.AddScoped<OrderServiceClient>(sp =>
{
    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientfactory.CreateClient("HttpClient");
    return new OrderServiceClient(httpClient);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "ReliZone.Web.Cookie";
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
