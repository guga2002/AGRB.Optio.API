using AutoMapper;
using Microsoft.Extensions.Logging;
using Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Services.TransactionRelated
{
    public class TransactionRelatedService(IUniteOfWork work, IMapper map, ILogger<TransactionRelatedService> log)
        : AbstractService<TransactionRelatedService>(work, map, log), ITransactionRelatedService
    {
        #region AddAsync
        public async Task<long> AddAsync(ChannelModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.ChannelType))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }

                var mapChannels = mapper.Map<Channels>(entity);
                if (mapChannels is null) return -1;
                var res = await work.ChannelRepository.AddAsync(mapChannels);
                logger.LogInformation($"{entity.ChannelType} is successfully added", DateTime.Now.ToShortDateString());
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<long> AddAsync(CategoryModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrEmpty(entity.TransactionCategory))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }

                var mapCategory = mapper.Map<Category>(entity);
                if (mapCategory is null) return -1;
                var res = await work.CategoryOfTransactionRepository.AddAsync(mapCategory);
                logger.LogInformation($"{entity.TransactionCategory} is successfully added",
                    DateTime.Now.ToShortDateString());
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<long> AddAsync(TransactionTypeModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.TransactionName))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }

                var mapTypeOfTransaction = mapper.Map<TypeOfTransaction>(entity);
                if (mapTypeOfTransaction is null) return -1;
                var res = await work.TypeOfTransactionRepository.AddAsync(mapTypeOfTransaction);
                logger.LogInformation($"{entity.TransactionName} is successfully added",
                    DateTime.Now.ToShortDateString());
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
        public async Task<IEnumerable<ChannelModel>> GetAllActiveAsync(ChannelModel Identify)
        {
            try
            {
                var activeChannelAsync = await work.ChannelRepository.GetAllActiveChannelAsync();
                if (activeChannelAsync is null) return new List<ChannelModel>();
                var mapped = mapper.Map<IEnumerable<ChannelModel>>(activeChannelAsync);
                return mapped;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetAllActiveAsync(CategoryModel identify)
        {
            try
            {
                var activeAsync = await work.CategoryOfTransactionRepository.GetAllActiveAsync();
                if (activeAsync is null) return new List<CategoryModel>();
                var mapped = mapper.Map<IEnumerable<CategoryModel>>(activeAsync);
                return mapped;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public async  Task<IEnumerable<TransactionTypeModel>> GetAllActiveAsync(TransactionTypeModel identify)
        {
            try
            {
                var activeTypeOfTransactionAsync =
                    await work.TypeOfTransactionRepository.GetAllActiveTypeOfTransactionAsync();
                if (activeTypeOfTransactionAsync is null) return new List<TransactionTypeModel>();
                var mapped = mapper.Map<IEnumerable<TransactionTypeModel>>(activeTypeOfTransactionAsync);
                return mapped;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<ChannelModel>> GetAllAsync(ChannelModel identify)
        {
            try
            {
                var res = await work.ChannelRepository.GetAllAsync();
                if (res is null) return Enumerable.Empty<ChannelModel>();
                var mapChannelModel = mapper.Map<IEnumerable<ChannelModel>>(res);
                return mapChannelModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync(CategoryModel identify)
        {

            try
            {
                var res = await work.CategoryOfTransactionRepository.GetAllAsync();
                if (res is null) return Enumerable.Empty<CategoryModel>();
                var mapCategoryModel = mapper.Map<IEnumerable<CategoryModel>>(res);
                return mapCategoryModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<TransactionTypeModel>> GetAllAsync(TransactionTypeModel identify)
        {
            try
            {
                var res = await work.TypeOfTransactionRepository.GetAllAsync();
                if (res is null) return Enumerable.Empty<TransactionTypeModel>();
                var mapTransactionTypeModel = mapper.Map<IEnumerable<TransactionTypeModel>>(res);
                return mapTransactionTypeModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetByIdAsync

        public async Task<ChannelModel> GetByIdAsync(long id, ChannelModel identify)
        {
            try
            {
                var res = await work.ChannelRepository.GetByIdAsync(id)
                          ?? throw new ItemNotFoundException($"Channel by id: {id} not found");
                var mapChannelModel = mapper.Map<ChannelModel>(res);
                return mapChannelModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<CategoryModel> GetByIdAsync(long id, CategoryModel identify)
        {
            try
            {
                var res = await work.CategoryOfTransactionRepository.GetByIdAsync(id) 
                    ?? throw new ItemNotFoundException($"Category by id: {id} not found");
                   
                var mapCategoryModel = mapper.Map<CategoryModel>(res);
                return mapCategoryModel;
            
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public  async Task<TransactionTypeModel> GetByIdAsync(long id, TransactionTypeModel identify)
        {
            try
            {
                var res = await work.TypeOfTransactionRepository.GetByIdAsync(id)
                          ?? throw new ItemNotFoundException($"Transaction Type by id: {id} not found");
            
                var mapTransactionTypeModel = mapper.Map<TransactionTypeModel>(res);
                return mapTransactionTypeModel;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region RemoveAsync
        public async Task<bool> RemoveAsync(long id, ChannelModel identity)
        {
            try
            {
                var channels = await work.ChannelRepository.GetByIdAsync(id);
                if (channels is null) return false;
                var mapped = mapper.Map<Channels>(channels);
                if (mapped is null) return false;
                var res = await work.ChannelRepository.RemoveAsync(mapped);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<bool> RemoveAsync(long id, CategoryModel identity)
        {
            try
            {
                var category = await work.CategoryOfTransactionRepository.GetByIdAsync(id);
                if (category is null) return false;

                var mapped = mapper.Map<Category>(category);
                if (mapped is null) return false;
                var res = await work.CategoryOfTransactionRepository.RemoveAsync(mapped);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public  async Task<bool> RemoveAsync(long id, TransactionTypeModel identity)
        {
            try
            {
                var typeOfTransaction = await work.TypeOfTransactionRepository.GetByIdAsync(id);
                if (typeOfTransaction is null) return false;
                var mapped = mapper.Map<TypeOfTransaction>(typeOfTransaction);
                if (mapped is null) return false;
                var res = await work.TypeOfTransactionRepository.RemoveAsync(mapped);
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
        public async Task<bool> SoftDeleteAsync(long id, ChannelModel identify)
        {
            try
            {
                var res = await  work.ChannelRepository.SoftDeleteAsync(id);
                return res;
            }
            catch (Exception exp)
            {
                logger.LogError(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public async  Task<bool> SoftDeleteAsync(long id, CategoryModel identify)
        {
            try
            {
                var res = await  work.CategoryOfTransactionRepository.SoftDeleteAsync(id);
                return res;
            }
            catch (Exception exp)
            {
                logger.LogError(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public async  Task<bool> SoftDeleteAsync(long id, TransactionTypeModel identify)
        {
            try
            {
                var res = await work.TypeOfTransactionRepository.SoftDeleteAsync(id);
                return res;
            }
            catch (Exception exp)
            {
                logger.LogError(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(long id, ChannelModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.ChannelType))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapped = mapper.Map<Channels>(entity);
                if (mapped is not null)
                {
                    return await work.ChannelRepository.UpdateAsync(id,mapped);
                }
                return false;
            }
            catch (Exception exp)
            {
                logger.LogError(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public  async Task<bool> UpdateAsync(long id, CategoryModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.TransactionCategory))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }

                var mapped = mapper.Map<Category>(entity);
                if (mapped is null) return false;
                return await work.CategoryOfTransactionRepository.UpdateAsync(id, mapped);
            }
            catch (Exception exp)
            {
                logger.LogError(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id,TransactionTypeModel entity)
        {

            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.TransactionName))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapped = mapper.Map<TypeOfTransaction>(entity);
                if (mapped is null) return false;
                return await work.TypeOfTransactionRepository.UpdateAsync(id,mapped);
             
            }
            catch (Exception exp)
            {
                logger.LogError(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion
    }
}
