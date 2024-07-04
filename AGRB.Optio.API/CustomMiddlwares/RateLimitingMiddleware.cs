namespace AGRB.Optio.API.CustomMiddlwares
{
    using System.Net;

    namespace AGRB.Optio.API.CustomMiddlwares
    {
        /// <summary>
        /// Middleware for rate limiting client requests based on IP address.
        /// </summary>
        public class RateLimitingMiddleware
        {
            private readonly RequestDelegate _next;

            private static readonly Dictionary<string, ClientRequestInfo> _clients = new Dictionary<string, ClientRequestInfo>();

            private readonly int _maxRequests = 10;
            private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1);

            /// <summary>
            /// Initializes a new instance of the <see cref="RateLimitingMiddleware"/> class.
            /// </summary>
            /// <param name="next">The next middleware in the pipeline.</param>
            public RateLimitingMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            /// <summary>
            /// Invokes the middleware asynchronously.
            /// </summary>
            /// <param name="context">The HTTP context for the current request.</param>
            /// <returns>A task that represents the completion of request processing.</returns>
            public async Task InvokeAsync(HttpContext context)
            {
                var clientIp = context.Connection.RemoteIpAddress.ToString();

                if (!_clients.ContainsKey(clientIp))
                {
                    _clients[clientIp] = new ClientRequestInfo
                    {
                        RequestCount = 1,
                        ExpiryTime = DateTime.Now.Add(_timeWindow)
                    };
                }
                else
                {
                    var clientInfo = _clients[clientIp];

                    if (clientInfo.ExpiryTime > DateTime.Now)
                    {
                        if (clientInfo.RequestCount >= _maxRequests)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                            await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                            return;
                        }
                        else
                        {
                            clientInfo.RequestCount++;
                        }
                    }
                    else
                    {
                        clientInfo.RequestCount = 1;
                        clientInfo.ExpiryTime = DateTime.Now.Add(_timeWindow);
                    }
                }

                await _next(context);
            }

            /// <summary>
            /// Information about client requests for rate limiting.
            /// </summary>
            private class ClientRequestInfo
            {
                /// <summary>
                /// Gets or sets the number of requests made by the client.
                /// </summary>
                public int RequestCount { get; set; }

                /// <summary>
                /// Gets or sets the time when the client's request count will be reset.
                /// </summary>
                public DateTime ExpiryTime { get; set; }
            }
        }
    }

}
