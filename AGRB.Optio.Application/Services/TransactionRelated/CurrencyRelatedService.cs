using AutoMapper;
using Microsoft.Extensions.Logging;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Services.TransactionRelated
{
    public class CurrencyRelatedService : AbstractService<CurrencyRelatedService>, ICurrencyRelatedService
    {
        public CurrencyRelatedService(IUniteOfWork work, IMapper map, ILogger<CurrencyRelatedService> log) : base(work, map, log)
        {
        }

        #region AddAsync
        public async Task<long> AddAsync(CurrencyModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.CurrencyCode) || string.IsNullOrEmpty(entity.NameOfValute))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapp = mapper.Map<Currency>(entity);
                if (mapp is not null)
                {
                    var res = await work.CurrencyRepository.AddAsync(mapp);
                    logger.LogInformation($"{entity.NameOfValute} is successfully added", DateTime.Now.ToShortDateString());
                    return res;
                }
                return -1;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }


        public async Task<long> AddAsync(ValuteModel entity)
        {
            try
            {
                if (entity is null || entity.CurrencyID < 0 || entity.ExchangeRate < 0 || string.IsNullOrEmpty(entity.DateOfValuteCourse.ToString()))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapp = mapper.Map<ValuteCourse>(entity);
                if (mapp is not null)
                {
                    var res = await work.ValuteCourse.AddAsync(mapp);
                    logger.LogInformation($"{entity.CurrencyID} is successfully added", DateTime.Now.ToShortDateString());
                    return res;
                }
                return -1;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetAllActiveAsync
        public async Task<IEnumerable<CurrencyModel>> GetAllActiveAsync(CurrencyModel Identify)
        {
            try
            {
                var res = await work.CurrencyRepository.GetAllActiveAsync();
                if (res is not null)
                {
                    var mapp = mapper.Map<IEnumerable<CurrencyModel>>(res);
                    return mapp;
                }
                else
                {
                    return Enumerable.Empty<CurrencyModel>();
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<ValuteModel>> GetAllActiveAsync(ValuteModel Identify)
        {
            try
            {
                var res = await work.ValuteCourse.GetAllActiveValuteAsync();
                if(res is not null)
                {
                    var mapp=mapper.Map<IEnumerable<ValuteModel>>(res);
                    return mapp;
                }
                return Enumerable.Empty<ValuteModel>();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message,ex.StackTrace,DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<CurrencyModel>> GetAllAsync(CurrencyModel Identify)
        {
            try
            {
                var res = await work.CurrencyRepository.GetAllAsync();
                if (res is not null)
                {
                    var mapp = mapper.Map<IEnumerable<CurrencyModel>>(res);
                    return mapp;
                }
                return Enumerable.Empty<CurrencyModel>();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<ValuteModel>> GetAllAsync(ValuteModel Identify)
        {
            try
            {
                var res = await work.ValuteCourse.GetAllAsync();
                if(res is not null)
                {
                    var mapp = mapper.Map<IEnumerable<ValuteModel>>(res);
                    return mapp;
                }
                else
                {
                    return Enumerable.Empty<ValuteModel>();
                }

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetByIdAsync

        public async Task<CurrencyModel> GetByIdAsync(int id, CurrencyModel Identify)
        {
            try
            {
                var res = await work.CurrencyRepository.GetByIdAsync(id);
                if (res is not null)
                {
                    var mapp = mapper.Map<CurrencyModel>(res);
                    return mapp;
                }
                else
                {
                    throw new ItemNotFoundException($"Currency with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<ValuteModel> GetByIdAsync(long id, ValuteModel Identify)
        {
            try
            {
                var res =await work.ValuteCourse.GetByIdAsync(id);
                if (res is not null)
                {
                    var mapp=mapper.Map<ValuteModel>(res);
                    return mapp;
                }
                else
                {
                    throw new ItemNotFoundException($"Valute with ID {id} not found.");
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

        public async Task<bool> RemoveAsync(int Id,CurrencyModel identity)
        {
            try
            {
                var currency = await work.CurrencyRepository.GetByIdAsync(Id);

                if (currency is not null)
                {
                    var mapp = mapper.Map<Currency>(currency);
                    if (mapp is not null)
                    {
                        var res = await work.CurrencyRepository.RemoveAsync(mapp);
                        return res;
                    }
                }
                throw new ArgumentException("no such entity exist");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message,ex.StackTrace,DateTime.Now.ToShortTimeString());  
                throw;
            }
        }

        public async Task<bool> RemoveAsync(long Id,ValuteModel identity)
        {
            try
            {
                var valute = await work.ValuteCourse.GetByIdAsync(Id);
                if (valute is not null)
                {
                    var mapp = mapper.Map<ValuteCourse>(valute);
                    if (mapp is not null)
                    {
                        var res = await work.ValuteCourse.RemoveAsync(mapp);
                        return res;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(int id, CurrencyModel Identify)
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

        public async Task<bool> SoftDeleteAsync(long id, ValuteModel Identify)
        {
            try
            {
                var res = await work.ValuteCourse.SoftDeleteAsync(id);
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
                if(entity == null || string.IsNullOrWhiteSpace(entity.NameOfValute) || string.IsNullOrWhiteSpace(entity.CurrencyCode))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapp=mapper.Map<Currency> (entity);
                if (mapp != null)
                {
                    var res=work.CurrencyRepository.UpdateAsync(id,mapp);
                    return res;
                }
                else
                {
                    throw new ItemNotFoundException($"Currency {entity.NameOfValute} not found");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public Task<bool> UpdateAsync(long id, ValuteModel entity)
        {
            try
            {
                if (entity == null || string.IsNullOrWhiteSpace(entity.DateOfValuteCourse.ToString()) || entity.CurrencyID<0||entity.ExchangeRate<0)
                {
                    throw new OptioGeneralException("Entity can not be null and currency id must be > 0 and  exchange rate must be > 0");
                }
                var mapp = mapper.Map<ValuteCourse>(entity);
                if (mapp != null)
                {
                    var res = work.ValuteCourse.UpdateAsync(id,mapp);
                    return res;
                }
                else
                {
                    throw new ItemNotFoundException($"Valute with currecny id {entity.CurrencyID} not found");
                }

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
