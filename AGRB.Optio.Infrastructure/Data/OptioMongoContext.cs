using AGRB.Optio.Domain.Entities;
using MongoDB.Driver;
namespace RGBA.Optio.Core.Data
{
    public sealed class OptioMongoContext
    {
        private readonly MongoClient client;
        public IMongoCollection<Log> UserLogs { get; set; }
        public OptioMongoContext()
        {
            client = new MongoClient();
            var database = client.GetDatabase("LogRGBA");
            UserLogs=database.GetCollection<Log>("LogsUserActions");
        }
    }
}
