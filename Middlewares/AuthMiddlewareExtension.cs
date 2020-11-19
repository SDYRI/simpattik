using Microsoft.AspNetCore.Builder;

namespace TasikmalayaKota.Simpatik.Web.Middlewares
{
    public static class AuthMiddlewareExtension
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
