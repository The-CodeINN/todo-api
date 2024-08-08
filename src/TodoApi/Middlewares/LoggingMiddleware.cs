using System.Diagnostics;

namespace TodoApi.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var requestName = request.Path;

            _logger.LogInformation("[START] Handling request: {RequestName} with data: {Request}", requestName, request);

            var timer = Stopwatch.StartNew();

            await _next(context);

            timer.Stop();

            if (timer.Elapsed.Seconds > 3)
            {
                _logger.LogWarning("[PERFORMANCE] The request {RequestName} took {ElapsedTime} seconds.", requestName, timer.Elapsed.Seconds);
            }

            _logger.LogInformation("[END] Handled {RequestName} with response: {Response}", requestName, context.Response);
        }
    }
}
