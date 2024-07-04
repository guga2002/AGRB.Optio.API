using AGRB.Optio.Application.Interfaces.InterfacesForTransaction;
using AGRB.Optio.Application.Models;

namespace AGRB.Optio.Application.Interfaces
{
    public interface ICurrencyRelatedService : IAddInfo<CurrencyModel>, IAddInfo<ExchangeRateModel>,
        IGetInfo<CurrencyModel, int>, IGetInfo<ExchangeRateModel, long>,
        IRemoveInfo<CurrencyModel, int>, IRemoveInfo<ExchangeRateModel, long>,
        IUpdateInfo<CurrencyModel, int>, IUpdateInfo<ExchangeRateModel, long>
    {

    }
}
