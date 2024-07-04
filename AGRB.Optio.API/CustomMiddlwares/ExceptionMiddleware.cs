using System.Net;

namespace AGRB.Optio.API.CustomMiddlwares
{
    /// <summary>
    /// Middleware for handling exceptions and logging errors.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger instance for logging errors.</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong: {ex}");
                    await HandleError(context, ex);
                }

                if (context.Response.StatusCode == 404)
                {
                    await HandleError(context, new Exception("No page found, need authorization"));
                    _logger.LogError($"No page found, need authorization");
                }

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        /// <summary>
        /// Handles the error by writing an error response.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="ex">The exception that occurred.</param>
        /// <returns>A task that represents the completion of error handling.</returns>
        private Task HandleError(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
                Detailed = ex.Source
            };

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }
}
