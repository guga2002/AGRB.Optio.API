using Optio.Core.Interfaces;
using RGBA.Optio.Core.Entities;

namespace RGBA.Optio.Core.Interfaces
{
    public interface IExchangeRate:ICrudRepo<ExchangeRate, long>
    {
        Task<IEnumerable<ExchangeRate>> GetAllActiveRateAsync();
    }
}
