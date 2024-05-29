using RGBA.Optio.Domain.Interfaces.InterfacesForTransaction;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Interfaces
{
    public interface ICurrencyRelatedService:IAddInfo<CurrencyModel>,IAddInfo<ExchangeRateModel>,
        IGetInfo<CurrencyModel, int>,IGetInfo<ExchangeRateModel, long>,
        IRemoveInfo<CurrencyModel,int>,IRemoveInfo<ExchangeRateModel, long>,
        IUpdateInfo<CurrencyModel,int>,IUpdateInfo<ExchangeRateModel, long>
    {

    }
}
