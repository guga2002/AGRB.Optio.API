using Optio.Core.Entities;

namespace Optio.Core.Interfaces
{
    public interface ITransactionRepo:ICrudRepo<Transaction, long>
    {
        Task<IEnumerable<Transaction>> GetAllWithDetailsAsync();

        Task<Transaction> GetByIdWithDetailsAsync(long Id);

        Task<IEnumerable<Transaction>> GetAllActiveAsync();
    }
}
