using MongoDB.Bson;

namespace AGRB.Optio.Domain.Entities
{
    public class Log:BsonDocument
    {
        public string? LogLevel { get; set; }

        public string? Message { get; set; }

    }
}
