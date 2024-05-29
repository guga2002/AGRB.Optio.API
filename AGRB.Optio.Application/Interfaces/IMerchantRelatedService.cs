using RGBA.Optio.Domain.Interfaces.InterfacesForTransaction;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Interfaces
{
    public interface IMerchantRelatedService:IAddInfo<MerchantModel>,IAddInfo<LocationModel>,
        IGetInfo<LocationModel, long>,IGetInfo<MerchantModel,long>,
        IRemoveInfo<LocationModel, long>,IRemoveInfo<MerchantModel, long>,
        IUpdateInfo<LocationModel, long>,IUpdateInfo<MerchantModel, long>
    {
        Task<bool> AssignLocationToMerchant(long merchantId, long locationId);
    }
}
