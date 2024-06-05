using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AGRB.Optio.Domain.Entities
{
    public class Logs
    {
        public Logs()
        {
        }

        public Logs(string? logLevel, string? message)
        {
            LogLevel = logLevel;
            Message = message;
        }

        [BsonId]
        public ObjectId Id { get; set; }    
        public string? LogLevel { get; set; }

        public string? Message { get; set; }

    }
}
