using Optio.Core.Entities;
using RGBA.Optio.Core.Entities;

namespace RGBA.Optio.Core.Interfaces
{
    public interface ILocationToMerchantRepository
    {
        Task<Location> GetLocationIdByMerchantIdAsync(long merchantId);
        Task<IEnumerable<LocationToMerchant>> GetAllLocationToMerchant();
    }
}
