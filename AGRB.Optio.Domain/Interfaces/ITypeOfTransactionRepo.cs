using Optio.Core.Entities;

namespace Optio.Core.Interfaces
{
    public interface ITypeOfTransactionRepo:ICrudRepo<TypeOfTransaction, long>
    {
        Task<IEnumerable<TypeOfTransaction>> GetAllActiveTypeOfTransactionAsync();
    }
}
