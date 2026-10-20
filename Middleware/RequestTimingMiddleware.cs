using System.Diagnostics;

namespace TaskManager.Middleware;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopper = Stopwatch.StartNew();

        await next(context);

        stopper.Stop();
        logger.LogInformation(
         "{Method} {Path} responded {StatusCode} in {ElapsedMs} ms", 
         context.Request.Method, context.Request.Path, context.Response.StatusCode, stopper.ElapsedMilliseconds);
    }
}