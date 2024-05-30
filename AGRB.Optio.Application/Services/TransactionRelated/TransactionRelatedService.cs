using AutoMapper;
using Microsoft.Extensions.Logging;
using Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;
using System.Numerics;

namespace RGBA.Optio.Domain.Services.TransactionRelated
{
    public class TransactionRelatedService : AbstractService<TransactionRelatedService>, ITransactionRelatedService
    {
        public TransactionRelatedService(IUniteOfWork work, IMapper map, ILogger<TransactionRelatedService> log) : base(work, map, log)
        {
        }

        #region AddAsync
        public async Task<long> AddAsync(ChanellModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.ChannelType))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapp = mapper.Map<Channels>(entity);
                if (mapp is not null)
                {
                    var res = await work.ChanellRepository.AddAsync(mapp);
                    logger.LogInformation($"{entity.ChannelType} is successfully added", DateTime.Now.ToShortDateString());
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

        public async Task<long> AddAsync(CategoryModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrEmpty(entity.TransactionCategory))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapped = mapper.Map<Category>(entity);
                if (mapped is not null)
                {
                    var res = await work.CategoryOfTransactionRepository.AddAsync(mapped);
                    logger.LogInformation($"{entity.TransactionCategory} is successfully added", DateTime.Now.ToShortDateString());
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

        public async Task<long> AddAsync(TransactionTypeModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.TransactionName))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var mapp = mapper.Map<TypeOfTransaction>(entity);
                if (mapp is not null)
                {
                    var res = await work.TypeOfTransactionRepository.AddAsync(mapp);
                    logger.LogInformation($"{entity.TransactionName} is successfully added", DateTime.Now.ToShortDateString());
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
        public async Task<IEnumerable<ChanellModel>> GetAllActiveAsync(ChanellModel Identify)
        {
            try
            {
                var allChanell = await work.ChanellRepository.GetAllActiveChannelAsync();
                if(allChanell is not null)
                {
                    var mapped = mapper.Map<IEnumerable<ChanellModel>>(allChanell);
                    return mapped;
                }
                return new List<ChanellModel>();
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetAllActiveAsync(CategoryModel Identify)
        {
            try
            {
                var allChanell = await work.CategoryOfTransactionRepository.GetAllActiveAsync();
                if (allChanell is not null)
                {
                    var mapped = mapper.Map<IEnumerable<CategoryModel>>(allChanell);
                    return mapped;
                }
                return new List<CategoryModel>();
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public async  Task<IEnumerable<TransactionTypeModel>> GetAllActiveAsync(TransactionTypeModel Identify)
        {
            try
            {
                var allChanell = await work.TypeOfTransactionRepository.GetAllActiveTypeOfTransactionAsync();
                if (allChanell is not null)
                {
                    var mapped = mapper.Map<IEnumerable<TransactionTypeModel>>(allChanell);
                    return mapped;
                }
                return new List<TransactionTypeModel>();
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<ChanellModel>> GetAllAsync(ChanellModel Identify)
        {
            try
            {
                var res = await work.ChanellRepository.GetAllAsync();
                if (res is not null)
                {
                    var mapp = mapper.Map<IEnumerable<ChanellModel>>(res);
                    return mapp;
                }
                return Enumerable.Empty<ChanellModel>();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync(CategoryModel Identify)
        {

            try
            {
                var res = await work.CategoryOfTransactionRepository.GetAllAsync();
                if (res is not null)
                {
                    var mapp = mapper.Map<IEnumerable<CategoryModel>>(res);
                    return mapp;
                }
                return Enumerable.Empty<CategoryModel>();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<IEnumerable<TransactionTypeModel>> GetAllAsync(TransactionTypeModel Identify)
        {
            try
            {
                var res = await work.TypeOfTransactionRepository.GetAllAsync();
                if (res is not null)
                {
                    var mapp = mapper.Map<IEnumerable<TransactionTypeModel>>(res);
                    return mapp;
                }
                return Enumerable.Empty<TransactionTypeModel>();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }
        #endregion

        #region GetByIdAsync

        public async Task<ChanellModel> GetByIdAsync(long id, ChanellModel Identify)
        {
            try
            {
                var res = await work.ChanellRepository.GetByIdAsync(id);
                if (res is not null)
                {
                    var mapp = mapper.Map<ChanellModel>(res);
                    return mapp;
                }
                else
                {
                    throw new ItemNotFoundException($"Channel by id: {id} not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public async Task<CategoryModel> GetByIdAsync(long id, CategoryModel Identify)
        {
            try
            {
                var res = await work.CategoryOfTransactionRepository.GetByIdAsync(id);
                if (res is not null)
                {
                    var mapp = mapper.Map<CategoryModel>(res);
                    return mapp;
                }
                else
                {
                    throw new ItemNotFoundException($"Category by id: {id} not found");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                throw;
            }
        }

        public  async Task<TransactionTypeModel> GetByIdAsync(long id, TransactionTypeModel Identify)
        {
            try
            {
                var res = await work.TypeOfTransactionRepository.GetByIdAsync(id);
                if (res is not null)
                {
                    var mapp = mapper.Map<TransactionTypeModel>(res);
                    return mapp;
                }
                else
                {
                    throw new ItemNotFoundException($"Transaction Type by id: {id} not found");
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
        public async Task<bool> RemoveAsync(long Id, ChanellModel identity)
        {
            try
            {
                var chanell = await work.ChanellRepository.GetByIdAsync(Id);
                if (chanell is not null)
                {
                    var mapped = mapper.Map<Channels>(chanell);
                    if (mapped is not null)
                    {
                        var res = await work.ChanellRepository.RemoveAsync(mapped);
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

        public async Task<bool> RemoveAsync(long Id, CategoryModel identity)
        {
            try
            {
                 var category = await work.CategoryOfTransactionRepository.GetByIdAsync(Id);
                if (category is not null)
                {
                    var mapped = mapper.Map<Category>(category);
                    if (mapped is not null)
                    {
                        var res = await work.CategoryOfTransactionRepository.RemoveAsync(mapped);
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

        public  async Task<bool> RemoveAsync(long Id, TransactionTypeModel Identity)
        {
            try
            {
                var transactyp = await work.TypeOfTransactionRepository.GetByIdAsync(Id);
                if (transactyp is not null)
                {
                    var mapped = mapper.Map<TypeOfTransaction>(transactyp);
                    if (mapped is not null)
                    {
                        var res = await work.TypeOfTransactionRepository.RemoveAsync(mapped);
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
        public async Task<bool> SoftDeleteAsync(long id, ChanellModel Identify)
        {
            try
            {
                var res = await  work.ChanellRepository.SoftDeleteAsync(id);
                return res;
            }
            catch (Exception exp)
            {
                logger.LogError(exp.Message, exp.StackTrace);
                throw;
            }
        }

        public async  Task<bool> SoftDeleteAsync(long id, CategoryModel Identify)
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

        public async  Task<bool> SoftDeleteAsync(long id, TransactionTypeModel Identify)
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
        public async Task<bool> UpdateAsync(long id, ChanellModel entity)
        {
            try
            {
                if (entity is null || string.IsNullOrWhiteSpace(entity.ChannelType))
                {
                    throw new OptioGeneralException("Entity can not be null");
                }
                var map = mapper.Map<Channels>(entity);
                if (map is not null)
                {
                    return await work.ChanellRepository.UpdateAsync(id,map);
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
                var map = mapper.Map<Category>(entity);
                if (map is not null)
                {
                    return await work.CategoryOfTransactionRepository.UpdateAsync(id,map);
                }
                return false;
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
                var map = mapper.Map<TypeOfTransaction>(entity);
                if (map is not null)
                {
                    return await work.TypeOfTransactionRepository.UpdateAsync(id,map);
                }
                return false;
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
