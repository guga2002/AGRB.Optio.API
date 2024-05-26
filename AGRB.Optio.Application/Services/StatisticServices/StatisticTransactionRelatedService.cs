using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Linq;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces.StatisticInterfaces;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Models.ResponseModels;

namespace RGBA.Optio.Domain.Services.StatisticServices
{
    public class StatisticTransactionRelatedService : AbstractService<StatisticTransactionRelatedService>, IStatisticTransactionRelatedService
    {
        public StatisticTransactionRelatedService(IUniteOfWork work, IMapper mapper, ILogger<StatisticTransactionRelatedService> logger)
            : base(work, mapper, logger)
        {
        }

        #region GetAllTransactionBetweenDate
        public async Task<IEnumerable<TransactionModel>> GetAllTransactionBetweenDate(DateTime start, DateTime end)
        {
            try
            {
                var transactions = await work.TransactionRepository.GetAllWithDetailsAsync();
                if (!transactions.Any())
                {
                    throw new OptioGeneralException("No transactions exist in the database.");
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
                var transactions = await work.TransactionRepository.GetAllWithDetailsAsync();
                if (!transactions.Any())
                {
                    throw new OptioGeneralException("No transactions exist.");
                }

                var filteredTransactions = transactions.Where(t => t.IsActive && t.Date >= start && t.Date <= end).ToList();
                var groupedByCategory = filteredTransactions
                    .GroupBy(t => t.Category)
                    .Select(g => new CategoryResponseModel
                    {
                        TransactionTypeID = g.Key.TransactionTypeID,
                        TransactionCategory = g.Key.TransactionCategory,
                        TransactionCount = g.Count(),
                        TransactionVolume = g.Sum(t => t.AmountEquivalent)
                    }).ToList();

                return groupedByCategory;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An error occurred while retrieving the most popular categories.");
                throw;
            }
        }
        #endregion

        #region GetTransactionQuantityWithDateAsync
        public async Task<IEnumerable<TranscationQuantitiesWithDateModel>> GetTransactionQuantityWithDateAsync(DateTime start, DateTime end)
        {
            try
            {
                var transactions = await work.TransactionRepository.GetAllWithDetailsAsync();

                if (!transactions.Any())
                {
                    throw new OptioGeneralException("No transactions exist.");
                }

                var filteredTransactions = transactions.Where(t => t.IsActive && t.Date >= start && t.Date <= end).ToList();
                var groupedWithDate = filteredTransactions
                    .GroupBy(t => new { t.Date.Year, t.Date.Month, t.Date.Day })
                    .Select(g => new TranscationQuantitiesWithDateModel
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
