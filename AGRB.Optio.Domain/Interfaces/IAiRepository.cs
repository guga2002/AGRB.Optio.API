
using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface IAiRepository // Ai will make  transaction fraudence when percente is above 93%
    {
       
        public Task<double> MakePredictionAboutFraudulence(Transaction transaction);

        public Task<IEnumerable<Transaction>> GetTransactionsForReview(DateTime start,DateTime end);

        Task<bool> UpdateTransactionPrediction(Transaction transaction, bool isFraudulent);

        Task<IDictionary<string, IEnumerable<Transaction>>> GetFraudulentTransactionsByCategory(DateTime start,DateTime end);

        Task<bool> ClearFraudulentStatus(IEnumerable<Transaction> transactions);

        Task<IEnumerable<Transaction>> GetSuspiciousTransactions(DateTime start,DateTime end); //  when % is 50-60  not hight then 60

        Task TrainModel();
    }
}
