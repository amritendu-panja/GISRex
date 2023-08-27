using Web.Middlewares;

namespace Web.Extensions
{
    public static class ApplicationExtensions
    {
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
