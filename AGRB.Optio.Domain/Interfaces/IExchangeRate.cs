using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface IExchangeRate : ICrudRepo<ExchangeRate, long>
    {
        Task<IEnumerable<ExchangeRate>> GetAllActiveRateAsync();
    }
}
