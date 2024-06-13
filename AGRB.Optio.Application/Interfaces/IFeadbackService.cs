using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Domain.Entities;
using RGBA.Optio.Domain.Interfaces.InterfacesForTransaction;

namespace AGRB.Optio.Application.Interfaces
{
    public interface IFeadbackService:IAddInfo<FeadbackModel>,IGetInfo<FeadbackModel,long>,IRemoveInfo<FeadbackModel,long>
    {
    }
}
