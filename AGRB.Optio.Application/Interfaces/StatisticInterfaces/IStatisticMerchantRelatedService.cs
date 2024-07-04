using AGRB.Optio.Application.Models.ResponseModels;

namespace AGRB.Optio.Application.Interfaces.StatisticInterfaces
{
    public interface IStatisticMerchantRelatedService
    {
        Task<IEnumerable<MerchantResponseModel>> GetMostPopularMerchantsAsync(DateTime start, DateTime end);

        Task<IEnumerable<ChannelResponseModel>> GetMostPopularChannelAsync(DateTime start, DateTime end);

        Task<IEnumerable<LocationResponseModel>> GetMostPopularLocationAsync(DateTime start, DateTime end);
    }
}
