using Application.Helpers;
using Application.Repository;
using Common.Entities;
using Common.Mappings;
using Common.Settings;
using Infrastructure.Data.ApplicationDbContext.Repositories;
using Infrastructure.Data.Repository;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using Web.Middlewares;

namespace Web.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AppSettings settings)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
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
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IRepository<ApplicationLayer>, Repository<ApplicationLayer>>();
            services.AddTransient<IRepository<SecurityTokenLog>, Repository<SecurityTokenLog>>();
            services.AddTransient<IRepository<ApplicationUserDetails>, Repository<ApplicationUserDetails>>();
            services.AddTransient<IQueryRepository<CountryLookup>, QueryRepository<CountryLookup>>();
            services.AddTransient<IQueryRepository<StateLookup>, QueryRepository<StateLookup>>();
            services.AddTransient<IApplicationPartnerOrganizationRepository, ApplicationPartnerOrganizationRepository>();
            return services;
        }

        public static IServiceCollection AddMediatorSettings(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                Assembly.GetExecutingAssembly(),
                typeof(Infrastructure.MediatorDependency.MediatorStartup).Assembly,
                typeof(Application.MediatorDependency.MediatorStartup).Assembly,
                typeof(Common.MediatorDependency.MediatorStartup).Assembly));
            return services;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddTransient<SharedMapping>();
            services.AddTransient<IHashHelper, HashHelper>();
            services.AddTransient<ITokenHelper, JwtTokenHelper>();
            return services;
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseApiKeyHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyMiddleware>();
            return app;
        }
    }
}
