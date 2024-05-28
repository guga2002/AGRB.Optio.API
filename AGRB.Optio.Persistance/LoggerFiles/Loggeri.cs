using AGRB.Optio.Domain.Entities;
using Microsoft.Extensions.Logging;
using RGBA.Optio.Core.Data;

namespace RGBA.Optio.Domain.LoggerFiles
{
    public class Logger : ILogger
    {
        private readonly OptioMongoContext context = new();
        

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel is not (LogLevel.Information or LogLevel.Error or LogLevel.Critical)) return;
            var logs = new Logs(logLevel: logLevel.ToString(), message: formatter(state, exception));
            context.UserLogs.InsertOne(logs);
        }
    }
}
