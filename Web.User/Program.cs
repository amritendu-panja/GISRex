using Common.Settings;
using Web.User.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddMemoryCache();
builder.Services.AddApplicationHelpers();

AppSettings settings = new AppSettings();
builder.Configuration.Bind("AppSettings", settings);

builder.Services.AddApplicationSession(settings);

builder.Services.AddControllersWithViews();

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
