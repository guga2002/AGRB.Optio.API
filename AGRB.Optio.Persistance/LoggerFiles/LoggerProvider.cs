using Microsoft.Extensions.Logging;

namespace RGBA.Optio.Domain.LoggerFiles
{
    public class LoggerProvider : ILoggerProvider
    {
        private bool disposedValue;
        public ILogger CreateLogger(string categoryName)
        {
            return new Logger();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                disposing = false;
            }

            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
