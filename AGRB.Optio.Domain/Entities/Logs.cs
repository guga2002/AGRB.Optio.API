using MongoDB.Bson;

namespace AGRB.Optio.Domain.Entities
{
    public class Logs:BsonDocument
    {
        public Logs()
        {
        }

        public Logs(string? logLevel, string? message)
        {
            LogLevel = logLevel;
            Message = message;
        }

        public string? LogLevel { get; set; }

        public string? Message { get; set; }

    }
}
