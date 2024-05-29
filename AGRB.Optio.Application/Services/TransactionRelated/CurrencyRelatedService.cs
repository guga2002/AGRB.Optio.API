using AutoMapper;
using Microsoft.Extensions.Logging;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Services.TransactionRelated
{
    public class CurrencyRelatedService(IUniteOfWork work, IMapper map, ILogger<CurrencyRelatedService> log)
        : AbstractService<CurrencyRelatedService>(work, map, log), ICurrencyRelatedService
    {
        #region AddAsync
        public async Task<long> AddAsync(CurrencyModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.CurrencyCode) || string.IsNullOrEmpty(entity.NameOfCurrency))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapCurrency = mapper.Map<Currency>(entity);
                if (mapCurrency is null) return -1;
                var res = await work.CurrencyRepository.AddAsync(mapCurrency);
                logger.LogInformation($"{entity.NameOfCurrency} is successfully added", DateTime.Now.ToShortDateString());
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }


        public async Task<long> AddAsync(ExchangeRateModel entity)
        {
            try
            {
                if (entity is null || entity.CurrencyId < 0 || entity.ExchangeRate < 0 || string.IsNullOrEmpty(entity.DateOfExchangeRate.ToString()))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapExchangeRate = mapper.Map<ExchangeRate>(entity);
                if (mapExchangeRate is null) return -1;
                var res = await work.ExchangeRateRepository.AddAsync(mapExchangeRate);
                logger.LogInformation($"{entity.CurrencyId} is successfully added", DateTime.Now.ToShortDateString());
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetAllActiveAsync
        public async Task<IEnumerable<CurrencyModel>> GetAllActiveAsync(CurrencyModel identify)
        {
            try
            {
                var res = await work.CurrencyRepository.GetAllActiveAsync();
                var mapCurrencyModels = mapper.Map<IEnumerable<CurrencyModel>>(res);
                return mapCurrencyModels;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<ExchangeRateModel>> GetAllActiveAsync(ExchangeRateModel identify)
        {
            try
            {
                var res = await work.ExchangeRateRepository.GetAllActiveRateAsync();
                if (res is null) return Enumerable.Empty<ExchangeRateModel>();
                var mapExchangeRateModel = mapper.Map<IEnumerable<ExchangeRateModel>>(res);
                return mapExchangeRateModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message,ex.StackTrace,DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<CurrencyModel>> GetAllAsync(CurrencyModel identify)
        {
            try
            {
                var res = await work.CurrencyRepository.GetAllAsync();
                if (res is null) return Enumerable.Empty<CurrencyModel>();
                var mapCurrencyModel = mapper.Map<IEnumerable<CurrencyModel>>(res);
                return mapCurrencyModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<ExchangeRateModel>> GetAllAsync(ExchangeRateModel identify)
        {
            try
            {
                var res = await work.ExchangeRateRepository.GetAllAsync();
                if(res is null) return Enumerable.Empty<ExchangeRateModel>();
                var mapExchangeRateModel = mapper.Map<IEnumerable<ExchangeRateModel>>(res); 
                return mapExchangeRateModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetByIdAsync

        public async Task<CurrencyModel> GetByIdAsync(int id, CurrencyModel identify)
        {
            try
            {
                var res = await work.CurrencyRepository.GetByIdAsync(id);
                return mapper.Map<CurrencyModel>(res) ??
                           throw new ItemNotFoundException($"Currency with ID {id} not found.");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<ExchangeRateModel> GetByIdAsync(long id, ExchangeRateModel identify)
        {
            try
            {
                var res =await work.ExchangeRateRepository.GetByIdAsync(id);
                if (res is not null)
                {
                    var mapExchangeRateModel = mapper.Map<ExchangeRateModel>(res);
                    return mapExchangeRateModel;
                }
                else
                {
                    throw new ItemNotFoundException($"Exchange rate with ID {id} not found.");
                }

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message,ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(int id,CurrencyModel identity)
        {
            try
            {
                var currency = await work.CurrencyRepository.GetByIdAsync(id);

                if (currency is null) throw new ArgumentException("no such entity exist");
                var mapCurrency = mapper.Map<Currency>(currency);
                if (mapCurrency is null) throw new ArgumentException("no such entity exist");
                var res = await work.CurrencyRepository.RemoveAsync(mapCurrency);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message,ex.StackTrace,DateTime.Now.ToShortTimeString());  
                throw;
            }
        }

        public async Task<bool> RemoveAsync(long Id,ExchangeRateModel identity)
        {
            try
            {
                var exchange = await work.ExchangeRateRepository.GetByIdAsync(Id);
                if (exchange is null) return false;
                var mapExchangeRate = mapper.Map<ExchangeRate>(exchange);
                if (mapExchangeRate is null) return false;
                var res = await work.ExchangeRateRepository.RemoveAsync(mapExchangeRate);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(int id, CurrencyModel identify)
        {
            try
            {
                var res = await work.CurrencyRepository.SoftDeleteAsync(id);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<bool> SoftDeleteAsync(long id, ExchangeRateModel identify)
        {
            try
            {
                var res = await work.ExchangeRateRepository.SoftDeleteAsync(id);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region UpdateAsync

        public Task<bool> UpdateAsync(int id, CurrencyModel entity)
        {
            try
            {
                if(entity == null || string.IsNullOrWhiteSpace(entity.NameOfCurrency) || string.IsNullOrWhiteSpace(entity.CurrencyCode))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapCurrency = mapper.Map<Currency> (entity) ?? throw new ItemNotFoundException($"Currency {entity.NameOfCurrency} not found");
                var res =work.CurrencyRepository.UpdateAsync(id, mapCurrency); return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public Task<bool> UpdateAsync(long id, ExchangeRateModel entity)
        {
            try
            {
                if (entity == null || string.IsNullOrWhiteSpace(entity.DateOfExchangeRate.ToString()) || entity.CurrencyId<0||entity.ExchangeRate<0)
                {
                    throw new OptioGeneralException("Entity can not be null and currency id must be > 0 and  exchange rate must be > 0");
                }
                var mapExchangeRate = mapper.Map<ExchangeRate>(entity);
                if (mapExchangeRate is null) throw new ItemNotFoundException($"Exchange rate with currency id {entity.CurrencyId} not found");
                
                var res = work.ExchangeRateRepository.UpdateAsync(id,mapExchangeRate);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion
    }
}
