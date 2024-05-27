using MongoDB.Bson;
using MongoDB.Driver;
namespace RGBA.Optio.Core.Data
{
    public class OptioMongoContext
    {
        private readonly MongoClient client;
        public virtual IMongoCollection<BsonDocument> UserLogs { get; set; }
        public OptioMongoContext()
        {
            client = new MongoClient();
            var database = client.GetDatabase("LogOptioLogs");
            UserLogs=database.GetCollection<BsonDocument>("LogsUserActions");
        }
    }
}
