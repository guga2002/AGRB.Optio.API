using Optio.Core.Interfaces;
using RGBA.Optio.Core.Entities;

namespace RGBA.Optio.Core.Interfaces
{
    public interface ICurrencyRepository:ICrudRepo<Currency,int>
    {
        Task<IEnumerable<Currency>> GetAllActiveAsync();

    }
}
