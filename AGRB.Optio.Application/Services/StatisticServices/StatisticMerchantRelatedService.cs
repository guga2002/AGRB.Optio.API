using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces.StatisticInterfaces;
using RGBA.Optio.Domain.Models.ResponseModels;

namespace RGBA.Optio.Domain.Services.StatisticServices
{
    public class StatisticMerchantRelatedService : AbstractService<StatisticMerchantRelatedService>, IStatisticMerchantRelatedService
    {
        public StatisticMerchantRelatedService(IUniteOfWork work, IMapper map, ILogger<StatisticMerchantRelatedService> log) : base(work, map, log)
        {
        }

        #region GetMostPopularChannelAsync
        public async Task<IEnumerable<ChannelResponseModel>> GetMostPopularChannelAsync(DateTime start, DateTime end)
        {
            try
            {
                var trans = await work.TransactionRepository.GetAllAsync();
                var transInDate = trans.Where(i => i.Date >= start && i.Date <= end).ToList();
                if (transInDate.Count == 0)
                {
                    throw new OptioGeneralException("No transactions exist in the database.");
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

                    var channelList = channel.ToList();
                    var tasks = channelList.Select(async g =>
                    {
                        var channelDetails = await work.ChanellRepository.GetByIdAsync(g.channelId);
                        if (channelDetails == null)
                        {
                            throw new OptioGeneralException($"Channel with ID {g.channelId} not found.");
                        }
                        return new ChannelResponseModel
                        {
                            ChannelType = channelDetails.ChannelType,
                            Quantity = g.channelCount,
                            Volume = g.volume,
                            Average = g.volume / g.channelCount
                        };
                    });

                    var channels = await Task.WhenAll(tasks);

                    return channels.OrderByDescending(c => c.Volume);
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
            try
            {
                var trans = await work.TransactionRepository.GetAllAsync();
                var transInDate = trans.Where(i => i.Date >= start && i.Date <= end).ToList();
                if (trans.Count() == 0)
                {
                    throw new OptioGeneralException("No transactions exist in the database.");
                }
                else
                {
                    var filtredTrans = from tran in trans
                                       group tran by tran.MerchantId into merch
                                       select new
                                       {
                                           merchantId = merch.Key,
                                           quantity = merch.Count(),
                                           volume = merch.Sum(i => i.Amount)
                                       };
                    var merchantLocationTask = filtredTrans.Select(async mg =>
                    {
                        var locationId=await work.LocationToMerchantRepository.GetLocationIdByMerchantIdAsync(mg.merchantId);
                        var merchantDetails = await work.MerchantRepository.GetByIdAsync(mg.merchantId);
                        return new
                        {
                            LocationId = locationId,
                            MerchantId = mg.merchantId,
                            merchantName = merchantDetails.Name,
                            TransactionCount = mg.quantity,
                            TransactionAmount = mg.volume

                        };
                    });
                    var merchantLocations = await Task.WhenAll(merchantLocationTask);

                    var locationGroups = from i in merchantLocations
                                         group i by i.LocationId into loc
                                         select new
                                         {
                                             LocatId = loc.Key.Id,
                                             TransactCount = loc.Count(),
                                             TotalVolume = loc.Sum(i => i.TransactionAmount),
                                             merchants = loc.Select(ml => ml.merchantName).First(),
                                         };
                    var locationList=locationGroups.ToList();
                    var merchantList=merchantLocationTask.ToList();
                    var locationTask = locationList.Select(async lc =>
                    {
                        var locationDetails = await work.LocationRepository.GetByIdAsync(lc.LocatId);

                        return new LocationResponseModel
                        {
                            Location = locationDetails.LocationName,
                            Quantity = lc.TransactCount,
                            Volume = lc.TotalVolume,
                            Average = lc.TotalVolume / lc.TransactCount,
                            MerchantName = lc.merchants
                        };
                    });

                    var locations = await Task.WhenAll(locationTask);

                    return locations.OrderByDescending(l => l.Volume);

                }
            }
            catch (Exception)
            {

                throw;
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
                    throw new OptioGeneralException("No transactions exist in the database.");
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

                    var merchantlList = merchant.ToList();
                    var tasks = merchantlList.Select(async g =>
                    {
                        var merchantDetails = await work.MerchantRepository.GetByIdAsync(g.merchantId);
                        if (merchantDetails == null)
                        {
                            throw new OptioGeneralException($"merchant with ID {g.merchantId} not found.");
                        }
                        return new MerchantResponseModel
                        {
                            Name = merchantDetails.Name,
                            Quantity = g.merchantCount,
                            Volume = g.volume,
                            Average = g.volume / g.merchantCount
                        };
                    });

                    var merch = await Task.WhenAll(tasks);

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
