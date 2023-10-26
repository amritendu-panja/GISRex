﻿using Common.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using Web.User.Clients;
using Web.User.Helpers;
using Web.User.Middlewares;
using Web.User.Services;

namespace Web.User.Extensions
{
    public static class ApplicationExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseAuthRefreshMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthRefreshMiddleware>();
            return app;
        }

        public static IServiceCollection AddApplicationHelpers(this IServiceCollection services)
        {
            services.AddTransient<CacheHelper>();
            services.AddTransient<CacheKeyGenrator>();
            services.AddTransient<AuthHelper>();
            services.AddTransient<AuthService>();
            services.AddTransient<PartnerService>();
            services.AddTransient<Mapper>();
            services.AddTransient<LookupsService>();
            services.AddTransient<FileHelper>();
            services.AddTransient<ViewHelper>();
            return services;
        }

        public static IServiceCollection AddApplicationSession(this IServiceCollection services, AppSettings settings)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(settings.Cache.SessionTimeoutInHours);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AppSettings settings)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(settings.Security.Authentication.AccessTokenTTLInMinutes);
                    options.LoginPath = "/login";
                })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.RequireHttpsMetadata = false;
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
                });
            return services;
        }

        public static IServiceCollection AddRefitClients(this IServiceCollection services, AppSettings settings)
        {
            services
                .AddRefitClient<IAuthenticationClient>(provider => new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer
                    (
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }
                    )
                })
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.Security.Authentication.Issuer));

            services
                .AddRefitClient<ILookupsClient>(provider => new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer
                    (
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }
                    )
                })
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.Security.Authentication.Issuer));

			services
				.AddRefitClient<IPartnerApiClient>(provider => new RefitSettings
				{
					ContentSerializer = new NewtonsoftJsonContentSerializer
					(
						new JsonSerializerSettings
						{
							ContractResolver = new CamelCasePropertyNamesContractResolver()
						}
					)
				})
				.ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.Security.Authentication.Issuer));
			return services;
        }
    }
}
