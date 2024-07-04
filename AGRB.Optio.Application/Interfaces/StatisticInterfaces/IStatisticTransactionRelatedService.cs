using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.ResponseModels;

namespace AGRB.Optio.Application.Interfaces.StatisticInterfaces
{
    public interface IStatisticTransactionRelatedService
    {
        Task<IEnumerable<CategoryResponseModel>> GetMostPopularCategoryAsync(DateTime start, DateTime end);
        Task<IEnumerable<TransactionQuantitiesWithDateModel>> GetTransactionQuantityWithDateAsync(DateTime start, DateTime end);
        Task<IEnumerable<TransactionModel>> GetAllTransactionBetweenDate(DateTime start, DateTime end);
    }
}
