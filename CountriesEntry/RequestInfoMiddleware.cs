using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CountriesEntry
{
    public class RequestInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestInfoMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, RequestInfo requestInfo)
        {
            requestInfo.Username = context.User.Identity.Name;
            //context.Response.ContentType = "text/html";
            //context.Response.StatusCode = 200;
            //await context.Response.WriteAsync("Hello");
            await _next.Invoke(context);
        }
    }

    public static class UseRequestInfoMiddlewareBuilder
    {
        public static IApplicationBuilder UseRequestInfoMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestInfoMiddleware>();
        }
    }
}
