using Optio.Core.Entities;

namespace RGBA.Optio.Core.Interfaces
{
    public interface ILocationToMerchantRepository
    {
        Task<Location> GetLocationIdByMerchantIdAsync(long merchantId);
    }
}
