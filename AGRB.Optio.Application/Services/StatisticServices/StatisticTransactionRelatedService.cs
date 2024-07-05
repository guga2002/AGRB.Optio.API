using AGRB.Optio.Application.Interfaces.StatisticInterfaces;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.ResponseModels;
using AGRB.Optio.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AGRB.Optio.Domain.Custom_Exceptions;
using AGRB.Optio.Application.StaticFiles;

namespace AGRB.Optio.Application.Services.StatisticServices
{
    public class StatisticTransactionRelatedService(
        IUniteOfWork work,
        IMapper mapper,
        ILogger<StatisticTransactionRelatedService> logger)
        : AbstractService<StatisticTransactionRelatedService>(work, mapper, logger), IStatisticTransactionRelatedService
    {
        #region GetAllTransactionBetweenDate
        public async Task<IEnumerable<TransactionModel>> GetAllTransactionBetweenDate(DateTime start, DateTime end)
        {
            try
            {
                var transactions = await work.TransactionRepository.GetAllAsync();
                if (transactions is null || !transactions.Any())
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }

                var filteredTransactions = transactions.Where(t => t.IsActive && t.Date >= start && t.Date <= end).ToList();
                var mappedTransactions = mapper.Map<IEnumerable<TransactionModel>>(filteredTransactions);
                logger.LogInformation("Transaction information successfully retrieved.");
                return mappedTransactions;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " error while retrieving transactions between dates.");
                throw;
            }
        }
        #endregion

        #region GetMostPopularCategoryAsync
        public async Task<IEnumerable<CategoryResponseModel>> GetMostPopularCategoryAsync(DateTime start, DateTime end)
        {
            try
            {
                var transactions = await work.TransactionRepository.GetAllAsync();
                if (!transactions.Any())
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }

                var filteredTransactions = transactions.Where(t => t.IsActive && t.Date >= start && t.Date <= end).ToList();
                var category = from i in filteredTransactions
                               group i by i.CategoryId
                             into groupCategory
                               select new
                               {
                                   categoryId = groupCategory.Key,
                                   categoryCount = groupCategory.Count(),
                                   volume = groupCategory.Sum(i => i.AmountEquivalent)
                               };

                List<CategoryResponseModel> lst = new List<CategoryResponseModel>();
                var categoryList = category.ToList();
                foreach (var item in categoryList)
                {
                    var categoryDetails = await work.CategoryOfTransactionRepository.GetByIdAsync(item.categoryId);
                    var res = new CategoryResponseModel
                    {
                        TransactionCategory = categoryDetails.TransactionCategory,
                        TransactionCount = item.categoryCount,
                        TransactionVolume = item.volume,
                        Average = item.volume / item.categoryCount,

                    };
                    lst.Add(res);
                }

                return lst;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while retrieving the most popular categories.");
                throw;
            }
        }
        #endregion

        #region GetTransactionQuantityWithDateAsync
        public async Task<IEnumerable<TransactionQuantitiesWithDateModel>> GetTransactionQuantityWithDateAsync(DateTime start, DateTime end)
        {
            try
            {
                var transactions = await work.TransactionRepository.GetAllAsync();

                if (!transactions.Any())
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }

                var filteredTransactions = transactions.Where(t => t.IsActive && t.Date >= start && t.Date <= end).ToList();
                var groupedWithDate = filteredTransactions
                    .GroupBy(t => new { t.Date.Year, t.Date.Month, t.Date.Day })
                    .Select(g => new TransactionQuantitiesWithDateModel
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                        SubTotal = g.Sum(t => t.AmountEquivalent)
                    }).ToList();

                return groupedWithDate;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "error  while retrieving transaction quantities by date.");
                throw;
            }
        }
        #endregion
    }
}
