using Application.Repository;
using Common.Entities;
using Common.Mappings;
using Common.Settings;
using Infrastructure.Data.ApplicationDbContext;
using Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;
using Web.Extensions;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddTransient<SharedMapping>();
builder.Services.AddTransient<IRepository<ApplicationUser>, Repository<ApplicationUser>>();
builder.Services.AddTransient<IRepository<ApplicationLayer>, Repository<ApplicationLayer>>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly(),
    typeof(Infrastructure.MediatorDependency.MediatorStartup).Assembly,
    typeof(Application.MediatorDependency.MediatorStartup).Assembly,
    typeof(Common.MediatorDependency.MediatorStartup).Assembly));

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.UseGlobalExceptionHandler();
app.UseApiKeyHandler();

app.MapControllers();

app.Run();
