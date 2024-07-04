using AGRB.Optio.Domain.Entities;
using MongoDB.Driver;
namespace AGRB.Optio.Domain.Data
{
    public sealed class OptioMongoContext
    {
        private readonly MongoClient client;
        public IMongoCollection<Logs> UserLogs { get; set; }
        public OptioMongoContext()
        {
            client = new MongoClient();
            var database = client.GetDatabase("Log_Optio_Solution");
            UserLogs = database.GetCollection<Logs>("AuditLogs");
        }
    }
}
