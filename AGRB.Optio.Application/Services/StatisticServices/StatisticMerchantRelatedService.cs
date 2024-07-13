using AGRB.Optio.Application.Interfaces.StatisticInterfaces;
using AGRB.Optio.Application.Models.ResponseModels;
using AGRB.Optio.Application.StaticFiles;
using AGRB.Optio.Domain.Custom_Exceptions;
using AGRB.Optio.Domain.Interfaces;
using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Configuration;

namespace AGRB.Optio.Application.Services.StatisticServices
{
    public class StatisticMerchantRelatedService(
        IUniteOfWork work,
        IMapper map,
        IConfiguration Configure,
        ILogger<StatisticMerchantRelatedService> log)
        : AbstractService<StatisticMerchantRelatedService>(work, map, log), IStatisticMerchantRelatedService
    {

       
        #region GetMostPopularChannelAsync
        public async Task<IEnumerable<ChannelResponseModel>> GetMostPopularChannelAsync(DateTime start, DateTime end)
        {
            try
            {
                var trans = await work.TransactionRepository.GetAllAsync();
                var transInDate = trans.Where(i => i.Date >= start && i.Date <= end).ToList();
                if (transInDate.Count == 0)
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                else
                {
                    var channel = from i in transInDate
                                  group i by i.ChannelId into g
                                  select new
                                  {
                                      channelId = g.Key,
                                      channelCount = g.Count(),
                                      volume = g.Sum(i => i.AmountEquivalent)
                                  };

                    List<ChannelResponseModel> lst = new List<ChannelResponseModel>();
                    var channelList = channel.ToList();
                    foreach (var item in channelList)
                    {
                        var channelDetails = await work.ChannelRepository.GetByIdAsync(item.channelId);
                        var res = new ChannelResponseModel
                        {
                            ChannelType = channelDetails.ChannelType,
                            Quantity = item.channelCount,
                            Volume = item.volume,
                            Average = item.volume / item.channelCount,

                        };
                        lst.Add(res);
                    }
                    return lst;
                }

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " error while retrieving channel between dates.");
                throw;
            }

        }
        #endregion

        #region GetMostPopularLocationAsync
        public async Task<IEnumerable<LocationResponseModel>> GetMostPopularLocationAsync(DateTime start, DateTime end)
        {
            var connectionstring = Configure.GetConnectionString("OptiosString");
            using (var dbConnection = new SqlConnection(connectionstring))
            {
                await dbConnection.OpenAsync();
                var query = @"SELECT 
                            l.Location_Name AS Location,
                         SUM(CASE WHEN t.Date_Of_Transaction >= @start AND t.Date_Of_Transaction <= @end THEN 1 ELSE 0 END) AS Quantity
                          FROM 
                           LocationToMerchants ltm
                          INNER JOIN 
                            Locations l ON ltm.LocationId = l.Id
                           INNER JOIN 
                                 Merchants m ON ltm.MerchantId = m.Id
                             INNER JOIN 
                           Transactions t ON m.Id = t.MerchantId
                        GROUP BY 
                        l.Location_Name
                         ORDER BY 
                         Quantity DESC;";

                var LocationResponse = await dbConnection.QueryAsync<LocationResponseModel>(query,new
                {
                    start=start,
                    end=end
                });
                return LocationResponse;
            }
        }

        #endregion

        #region GetMostPopularMerchantsAsync
        public async Task<IEnumerable<MerchantResponseModel>> GetMostPopularMerchantsAsync(DateTime start, DateTime end)
        {
            try
            {
                var trans = await work.TransactionRepository.GetAllAsync();
                var transInDate = trans.Where(i => i.Date >= start && i.Date <= end).ToList();
                if (transInDate.Count == 0)
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                else
                {
                    var merchant = from i in transInDate
                                   group i by i.MerchantId into g
                                   select new
                                   {
                                       merchantId = g.Key,
                                       merchantCount = g.Count(),
                                       volume = g.Sum(i => i.Amount)
                                   };

                    var merchantList = merchant.ToList();
                    List<MerchantResponseModel> merch = new List<MerchantResponseModel>();
                    foreach (var item in merchantList)
                    {
                        var merchantDetails = await work.MerchantRepository.GetByIdAsync(item.merchantId);

                        var res = new MerchantResponseModel
                        {
                            Name = merchantDetails.Name,
                            Quantity = item.merchantCount,
                            Volume = item.volume,
                            Average = item.volume / item.merchantCount
                        };
                        merch.Add(res);
                    }
                    return merch.OrderByDescending(c => c.Volume);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, " error while retrieving channel between dates.");
                throw;
            }
        }
        #endregion
    }
}