using Optio.Core.Interfaces;

namespace RGBA.Optio.Core.Interfaces
{
    public interface IUniteOfWork:IDisposable
    {
        ICategoryRepo CategoryOfTransactionRepository { get; }

        IChannelRepo ChannelRepository { get; }

        ILocationRepo LocationRepository { get; }

        IMerchantRepo MerchantRepository { get; }

        ITransactionRepo TransactionRepository { get; }

        ITypeOfTransactionRepo TypeOfTransactionRepository { get; }
        
        ILocationToMerchantRepository LocationToMerchantRepository { get; }
       
        IExchangeRate ExchangeRateRepository { get; }

        ICurrencyRepository CurrencyRepository { get; }

        Task CheckAndCommitAsync();
    }
}
