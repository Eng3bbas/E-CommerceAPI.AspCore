using System;
using System.Security.Claims;
using System.Threading.Tasks;
using E_Commerce.Extensions;
using E_Commerce.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Http.Middleware
{
    public class VerifyTokenNotRevoked : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tokenManger = context.RequestServices.GetService<TokenManger>();
            var jti = Guid.Parse(context.User.FindFirstValue("jti")) ;
            var userId = context.User.GetUserId().Value;
            if (!tokenManger.IsTokenRevoked(userId,jti))
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = 401;
            context.Response.Headers["www-authenticate"] = "Token is revoked";
            await context.Response.WriteAsync("");
        }
    }

    public static class VerifyTokenNotRevokedExtensions
    {
        public static void UseVerifyTokenNotRevoked(this IApplicationBuilder application)
        {
            application.UseMiddleware<VerifyTokenNotRevoked>();
        }
    }
}