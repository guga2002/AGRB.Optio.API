using AGRB.Optio.Application.Interfaces.InterfacesForTransaction;
using AGRB.Optio.Application.Models;

namespace AGRB.Optio.Application.Interfaces
{
    public interface IMerchantRelatedService : IAddInfo<MerchantModel>, IAddInfo<LocationModel>,
        IGetInfo<LocationModel, long>, IGetInfo<MerchantModel, long>,
        IRemoveInfo<LocationModel, long>, IRemoveInfo<MerchantModel, long>,
        IUpdateInfo<LocationModel, long>, IUpdateInfo<MerchantModel, long>
    {
        Task<bool> AssignLocationToMerchant(long merchantId, long locationId);
    }
}
