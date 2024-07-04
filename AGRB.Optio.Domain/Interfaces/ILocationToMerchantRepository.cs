using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface ILocationToMerchantRepository
    {
        Task<Location> GetLocationIdByMerchantIdAsync(long merchantId);
        Task<IEnumerable<LocationToMerchant>> GetAllLocationToMerchant();
    }
}
