using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Models.ResponseModels;

namespace RGBA.Optio.Domain.Interfaces.StatisticInterfaces
{
    public interface IStatisticTransactionRelatedService
    {
        Task<IEnumerable<CategoryResponseModel>> GetMostPopularCategoryAsync(DateTime start, DateTime end);
        Task<IEnumerable<TranscationQuantitiesWithDateModel>> GetTransactionQuantityWithDateAsync(DateTime start, DateTime end);
        Task<IEnumerable<TransactionModel>> GetAllTransactionBetweenDate(DateTime start, DateTime end);
    }
}
