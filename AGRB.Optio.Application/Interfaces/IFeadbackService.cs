using AGRB.Optio.Application.Interfaces.InterfacesForTransaction;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Application.Interfaces
{
    public interface IFeadbackService:IAddInfo<FeadbackModel>,IGetInfo<FeadbackModel,long>,IRemoveInfo<FeadbackModel,long>
    {
    }
}
