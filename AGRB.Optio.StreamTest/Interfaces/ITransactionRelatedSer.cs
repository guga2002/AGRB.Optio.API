using AGRB.Optio.Domain.Entities;
using AGRB.Optio.StreamTest.DecerializerCLasses;

namespace AGRB.Optio.StreamTest.Interfaces
{
    public interface ITransactionRelatedSer
    {
        Task<bool> fillChannel();
        Task<bool> FillTypeOfTransaction();
        Task InsertCurrencies(List<CurrenciesResponse> cur);
        Task<bool> FillTransactions(int n);
        Task<bool> FillTransactionsBulk(int n);
        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<IEnumerable<Transaction>> GetAllTransactionsWithoutDapper();
    }
}
