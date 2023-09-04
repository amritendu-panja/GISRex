using Application.Helpers;
using Application.Repository;
using Common.Entities;
using Common.Mappings;
using Common.Settings;
using Infrastructure.Data.ApplicationDbContext;
using Infrastructure.Data.ApplicationDbContext.Repositories;
using Infrastructure.Data.Repository;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using Web.Extensions;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddTransient<SharedMapping>();
builder.Services.AddTransient<IHashHelper, HashHelper>();
builder.Services.AddTransient<ITokenHelper, JwtTokenHelper>();
builder.Services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
builder.Services.AddTransient<IRepository<ApplicationLayer>, Repository<ApplicationLayer>>();
builder.Services.AddTransient<IRepository<SecurityTokenLog>, Repository<SecurityTokenLog>>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly(),
    typeof(Infrastructure.MediatorDependency.MediatorStartup).Assembly,
    typeof(Application.MediatorDependency.MediatorStartup).Assembly,
    typeof(Common.MediatorDependency.MediatorStartup).Assembly));

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    AppSettings settings = new AppSettings();
    builder.Configuration.Bind("AppSettings", settings);
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.Security.Authentication.AccessTokenSecret)),
        ValidIssuer = settings.Security.Authentication.Issuer,
        ValidAudience = settings.Security.Authentication.Audience,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero
    };
    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context => {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
            }
            return Task.CompletedTask;
        }
    };
});

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
