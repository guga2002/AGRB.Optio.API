using AutoMapper;
using Microsoft.Extensions.Logging;
using Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Services.TransactionRelated
{
    public class MerchantRelatedService(IUniteOfWork work, IMapper map, ILogger<MerchantRelatedService> log)
        : AbstractService<MerchantRelatedService>(work, map, log), IMerchantRelatedService
    {
        #region AddAsync
        public async Task<long> AddAsync(MerchantModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.Name))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapMerchant = mapper.Map<Merchant>(entity);
                if (mapMerchant is null) return -1;
                var res = await work.MerchantRepository.AddAsync(mapMerchant);
                logger.LogInformation($"{entity.Name} is successfully added", DateTime.Now.ToShortDateString());
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<long> AddAsync(LocationModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrEmpty(entity.LocationName))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapLocation = mapper.Map<Location>(entity);
                if (mapLocation is not null)
                {
                    var res = await work.LocationRepository.AddAsync(mapLocation);
                    logger.LogInformation($"{entity.LocationName} is successfully added", DateTime.Now.ToShortDateString());
                    return res;
                }
                else { return -1; }

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region AssignLocationToMerchant
        public async Task<bool> AssignLocationToMerchant(long merchantId, long locationId)
        {
            try
            {
              var res= await  work.MerchantRepository.AssignLocationToMerchant(merchantId, locationId);
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

        public async Task<IEnumerable<LocationModel>> GetAllActiveAsync(LocationModel identify)
        {
            try
            {
                var res =await work.LocationRepository.GetAllActiveLocationAsync();
                if (res is null) return Enumerable.Empty<LocationModel>();
                var mapLocationModel = mapper.Map<IEnumerable<LocationModel>>(res);
                return mapLocationModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<MerchantModel>> GetAllActiveAsync(MerchantModel identify)
        {
            try
            {
                var res = await work.MerchantRepository.GetAllActiveMerchantAsync();
                if (res is null) return Enumerable.Empty<MerchantModel>();
                var mapMerchantModel = mapper.Map<IEnumerable<MerchantModel>>(res);
                return mapMerchantModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<LocationModel>> GetAllAsync(LocationModel identify)
        {
            try
            {
                var res = await work.LocationRepository.GetAllAsync();
                if (res is null) return Enumerable.Empty<LocationModel>();
                var mapLocationModel = mapper.Map<IEnumerable<LocationModel>>(res);
                return mapLocationModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }


        public async Task<IEnumerable<MerchantModel>> GetAllAsync(MerchantModel identify)
        {
            try
            {
                var res = await work.MerchantRepository.GetAllAsync();
                if (res is null) return Enumerable.Empty<MerchantModel>();
                var mapMerchantModel = mapper.Map<IEnumerable<MerchantModel>>(res);
                return mapMerchantModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<LocationModel> GetByIdAsync(long id, LocationModel identify)
        {
            try
            {
                var res = await work.LocationRepository.GetByIdAsync(id);
                if (res is null) throw new ItemNotFoundException($"Location with id: {id} not found");
                var mapLocationModel = mapper.Map<LocationModel>(res);
                return mapLocationModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<MerchantModel> GetByIdAsync(long id, MerchantModel identify)
        {
            try
            {
                var res = await work.MerchantRepository.GetByIdAsync(id);
                if (res is null) throw new ItemNotFoundException($"Merchant with id: {id} not found");
                var mapMerchantModel = mapper.Map<MerchantModel>(res);
                return mapMerchantModel;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(long id, LocationModel identity)
        {
            try
            {
                var location = await work.LocationRepository.GetByIdAsync(id);
                if (location is null) return false;
                var mapLocation = mapper.Map<Location>(location);
                if (mapLocation is null) return false;
                var res = await work.LocationRepository.RemoveAsync(mapLocation);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<bool> RemoveAsync(long id, MerchantModel identity)
        {
            try
            {
                var merchant = await work.MerchantRepository.GetByIdAsync(id);
                if (merchant is null) return false;
                var mapMerchant = mapper.Map<Merchant>(merchant);
                if (mapMerchant is null) return false;
                var res = await work.MerchantRepository.RemoveAsync(mapMerchant);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(long id, LocationModel identify)
        {
            try
            {
                var res =await work.LocationRepository.SoftDeleteAsync(id);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<bool> SoftDeleteAsync(long id, MerchantModel identify)
        {
            try
            {
                var res = await work.MerchantRepository.SoftDeleteAsync(id);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, LocationModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.LocationName))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }

                var mapLocation = mapper.Map<Location>(entity);
                if (mapLocation is null) throw new ItemNotFoundException($"{entity.LocationName} not found");
                var res = await work.LocationRepository.UpdateAsync(id, mapLocation);
                return res;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id,MerchantModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.Name))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }

                var mapMerchant = mapper.Map<Merchant>(entity);
                if (mapMerchant is null) throw new ItemNotFoundException($"{entity.Name} not found");
                var res = await work.MerchantRepository.UpdateAsync(id, mapMerchant);
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
