namespace WebApplication1.Middlewares
{
    public class RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        private static readonly object SyncRoot = new();

        private const int MaxRequests = 20;
        private static readonly TimeSpan Window = TimeSpan.FromSeconds(10);

        private static int _counter;
        private static DateTime _windowStartedAt = DateTime.UtcNow;

        public async Task InvokeAsync(HttpContext context)
        {
            // Static assets (Swagger UI) should not consume the API request budget.
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await next(context);
                return;
            }

            if (!TryAcquire())
            {
                logger.LogWarning(
                    "Rate limit exceeded for {Method} {Path}.",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers.RetryAfter = Window.TotalSeconds.ToString("0");
                await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
                return;
            }

            await next(context);
        }

        private static bool TryAcquire()
        {
            lock (SyncRoot)
            {
                var now = DateTime.UtcNow;

                if (now - _windowStartedAt >= Window)
                {
                    _windowStartedAt = now;
                    _counter = 0;
                }

                _counter++;
                return _counter <= MaxRequests;
            }
        }
    }
}
