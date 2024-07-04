namespace AGRB.Optio.API.CustomMiddlwares
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware asynchronously.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Handling request: {context.Request.Method} {context.Request.Path}");
            await _next(context);
            _logger.LogInformation($"Finished handling request: {context.Response.StatusCode}");
        }
    }
}
