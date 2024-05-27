using RGBA.Optio.Domain.Models.ResponseModels;

namespace RGBA.Optio.Domain.Interfaces.StatisticInterfaces
{
    public interface IStatisticMerchantRelatedService
    {
        Task<IEnumerable<MerchantResponseModel>> GetMostPopularMerchantsAsync(DateTime start, DateTime end);

        Task<IEnumerable<ChannelResponseModel>> GetMostPopularChannelAsync(DateTime start, DateTime end);

        Task<IEnumerable<LocationResponseModel>> GetMostPopularLocationAsync(DateTime start, DateTime end);
    }
}
