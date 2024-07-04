using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Services;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AGRB.Optio.Domain.Custom_Exceptions;
using AGRB.Optio.Application.StaticFiles;
using Microsoft.VisualBasic;

namespace AGRB.Optio.Application.Services.TransactionRelated
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
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                var mapCurrency = mapper.Map<Currency>(entity);
                if (mapCurrency is null) return -1;
                var res = await work.CurrencyRepository.AddAsync(mapCurrency);
                logger.LogInformation($"{entity.NameOfCurrency} {SuccessKeys.Success}", DateTime.Now.ToShortDateString());
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
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                var mapExchangeRate = mapper.Map<ExchangeRate>(entity);
                if (mapExchangeRate is null) return -1;
                var res = await work.ExchangeRateRepository.AddAsync(mapExchangeRate);
                logger.LogInformation($"{entity.CurrencyId} {SuccessKeys.warmateba}", DateTime.Now.ToShortDateString());
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
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
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
                if (res is null) return Enumerable.Empty<ExchangeRateModel>();
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
                           throw new ItemNotFoundException(ErrorKeys.NotFound);
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
                var res = await work.ExchangeRateRepository.GetByIdAsync(id);
                if (res is not null)
                {
                    var mapExchangeRateModel = mapper.Map<ExchangeRateModel>(res);
                    return mapExchangeRateModel;
                }
                else
                {
                    throw new ItemNotFoundException(ErrorKeys.NotFound);
                }

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(int id, CurrencyModel identity)
        {
            try
            {
                var currency = await work.CurrencyRepository.GetByIdAsync(id);

                if (currency is null) throw new ArgumentException(ErrorKeys.NotFound);
                var mapCurrency = mapper.Map<Currency>(currency);
                if (mapCurrency is null) throw new ArgumentException(ErrorKeys.NotFound);
                var res = await work.CurrencyRepository.RemoveAsync(mapCurrency);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<bool> RemoveAsync(long Id, ExchangeRateModel identity)
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
                if (entity == null || string.IsNullOrWhiteSpace(entity.NameOfCurrency) || string.IsNullOrWhiteSpace(entity.CurrencyCode))
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                var mapCurrency = mapper.Map<Currency>(entity) ?? throw new ItemNotFoundException($"{entity.NameOfCurrency}{ ErrorKeys.NotFound}");
                var res = work.CurrencyRepository.UpdateAsync(id, mapCurrency); return res;
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
                if (entity == null || string.IsNullOrWhiteSpace(entity.DateOfExchangeRate.ToString()) || entity.CurrencyId < 0 || entity.ExchangeRate < 0)
                {
                    throw new OptioGeneralException(ErrorKeys.currencyrelated);
                }
                var mapExchangeRate = mapper.Map<ExchangeRate>(entity);
                if (mapExchangeRate is null) throw new ItemNotFoundException(ErrorKeys.NotFound);

                var res = work.ExchangeRateRepository.UpdateAsync(id, mapExchangeRate);
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
