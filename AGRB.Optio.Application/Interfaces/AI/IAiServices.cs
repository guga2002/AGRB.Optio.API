using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.RequestModels;

namespace AGRB.Optio.Application.Interfaces.AI
{
    public interface IAiServices
    {
        Task<double> MakePredictionAboutFraudulence(TransactionModel transaction);

        Task<IEnumerable<TransactionModel>> GetTransactionsForReview(DateRangeRequestModel date);

        Task<bool> UpdateTransactionPrediction(TransactionModel transaction, bool isFraudulent);

        Task<IDictionary<string, IEnumerable<TransactionModel>>> GetFraudulentTransactionsByCategory(DateRangeRequestModel date);

        Task<bool> ClearFraudulentStatus(IEnumerable<TransactionModel> transactions);

        Task<IEnumerable<TransactionModel>> GetSuspiciousTransactions(DateRangeRequestModel date); //  when % is 50-60  not hight then 60

        Task TrainModel();
    }
}
