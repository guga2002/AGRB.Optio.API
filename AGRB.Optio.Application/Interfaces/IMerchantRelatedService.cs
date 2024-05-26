using RGBA.Optio.Domain.Interfaces.InterfacesForTransaction;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Interfaces
{
    public interface IMerchantRelatedService:IAddInfo<MerchantModel>,IAddInfo<locationModel>,
        IGetInfo<locationModel, long>,IGetInfo<MerchantModel,long>,
        IRemoveInfo<locationModel, long>,IRemoveInfo<MerchantModel, long>,
        IUpdateInfo<locationModel, long>,IUpdateInfo<MerchantModel, long>
    {
        Task<bool> AssignLocationtoMerchant(long Merchantid, long Locationid);
    }
}
