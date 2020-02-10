using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;

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

                        newBodyStream.Seek(0, SeekOrigin.Begin);
                        var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
                        newBodyStream.Seek(0, SeekOrigin.Begin);
                        responseBody = responseBody.Replace("\"\\\"", "\"").Replace("\\\"\"", "\"")
                            .Replace("\"{", "{").Replace("}\"", "}")
                            .Replace("\\\"", "\"");
                        var contentType = context.Response.ContentType;
                        var contentLength = responseBody != null ? Encoding.UTF8.GetByteCount(responseBody) : 0;

                        context.Response.Body = originalBodyStream;
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = contentType;
                        context.Response.ContentLength = contentLength;
                        await context.Response.WriteAsync(responseBody);
                    }
                    catch (Exception e)
                    {
                        context.Response.Body = originalBodyStream;
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        var bodyText = JsonSerializer.Serialize(new
                        {
                            statusCode = 500,
                            isError = true,
                            responseException = new
                            {
                                exceptionMessage = $"Stranger Error， {e.Message}"
                            }
                        });
                        context.Response.ContentLength = bodyText.Length;
                        await context.Response.WriteAsync(bodyText);
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
