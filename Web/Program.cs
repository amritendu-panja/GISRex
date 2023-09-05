using Common.Settings;
using Infrastructure.Data.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddHelpers();
builder.Services.AddRepositories();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddMediatorSettings();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers();

AppSettings settings = new AppSettings();
builder.Configuration.Bind("AppSettings", settings);

builder.Services.AddJwtAuthentication(settings);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseGlobalExceptionHandler();
app.UseApiKeyHandler();

app.MapControllers();

app.Run();
