using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface ITypeOfTransactionRepo : ICrudRepo<TypeOfTransaction, long>
    {
        Task<IEnumerable<TypeOfTransaction>> GetAllActiveTypeOfTransactionAsync();
    }
}
