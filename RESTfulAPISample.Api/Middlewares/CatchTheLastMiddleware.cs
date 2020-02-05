using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace RESTfulAPISample.Middleware
{
    public class CatchTheLastMiddleware
    {
        private readonly RequestDelegate _next;
        public IConfiguration _configuration { get; }
        public CatchTheLastMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsSwagger(context))
                await _next(context);
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var newBodyStream = new MemoryStream())
                {
                    try
                    {
                        context.Response.Body = newBodyStream;

                        await _next.Invoke(context);

                        var contentType = context.Response.ContentType;
                        var contentLength = context.Response.ContentLength;
                        newBodyStream.Seek(0, SeekOrigin.Begin);
                        var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
                        newBodyStream.Seek(0, SeekOrigin.Begin);

                        context.Response.Body = originalBodyStream;
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = contentType;
                        context.Response.ContentLength = contentLength;
                        await context.Response.WriteAsync(responseBody);
                    }
                    catch(Exception e)
                    {
                        if (newBodyStream.Length > 0)
                        {
                            newBodyStream.Seek(0, SeekOrigin.Begin);
                            await newBodyStream.CopyToAsync(originalBodyStream);
                        }
                        else
                        {
                            context.Response.Body = originalBodyStream;
                            context.Response.StatusCode = 200;
                            context.Response.ContentType = "application/json";
                            context.Response.ContentLength = 49;
                            await context.Response.WriteAsync("{\"larsson\":\"todo. finish this json result\"}");

                        }
                    }
                }
            }
        }

        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(new PathString("/swagger"));
        }
    }

    public static class CatchTheLastMiddlewareExt
    {
        public static IApplicationBuilder UseCatchTheLastMiddleware(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<CatchTheLastMiddleware>();

        }
    }
}
