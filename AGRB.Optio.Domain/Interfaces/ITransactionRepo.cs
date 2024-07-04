using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface ITransactionRepo : ICrudRepo<Transaction, long>
    {
        Task<IEnumerable<Transaction>> GetAllWithDetailsAsync();

        Task<Transaction> GetByIdWithDetailsAsync(long id);

        Task<IEnumerable<Transaction>> GetAllActiveAsync();

    }
}
