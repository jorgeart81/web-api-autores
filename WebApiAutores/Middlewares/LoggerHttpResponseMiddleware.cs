using System;

namespace WebApiAutores.Middlewares;

public static class LoggerHttpResponseMiddlewareExtension
{
  public static IApplicationBuilder UseLoggerHttpResponse(this IApplicationBuilder app)
  {
    return app.UseMiddleware<LoggerHttpResponseMiddleware>();
  }
}

public class LoggerHttpResponseMiddleware(RequestDelegate next)
{
  // Invoke or InvokeAsync
  public async Task InvokeAsync(HttpContext context, ILogger<LoggerHttpResponseMiddleware> logger)
  {
    using (var ms = new MemoryStream())
    {
      var originalBodyResponse = context.Response.Body;
      context.Response.Body = ms;

      await next(context);

      ms.Seek(0, SeekOrigin.Begin);
      var response = new StreamReader(ms).ReadToEnd();
      ms.Seek(0, SeekOrigin.Begin);

      await ms.CopyToAsync(originalBodyResponse);
      context.Response.Body = originalBodyResponse;

      logger.LogInformation(response);
    }
  }
}
