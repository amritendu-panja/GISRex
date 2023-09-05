using Common.Settings;
using Web.User.Extensions;
using Web.User.Helpers;
using Web.User.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<AuthHelper>();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<CacheHelper>();
builder.Services.AddTransient<CacheKeyGenrator>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

AppSettings settings = new AppSettings();
builder.Configuration.Bind("AppSettings", settings);

builder.Services.AddJwtAuthentication(settings);
builder.Services.AddRefitClients(settings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseGlobalExceptionHandler();
app.UseAuthRefreshMiddleware();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
