using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface ICurrencyRepository : ICrudRepo<Currency, int>
    {
        Task<IEnumerable<Currency>> GetAllActiveAsync();

    }
}
