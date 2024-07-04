using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AGRB.Optio.Domain.Custom_Exceptions;
using AGRB.Optio.Application.StaticFiles;

namespace AGRB.Optio.Application.Services
{
    public class TransactionService(IUniteOfWork work, IMapper map, ILogger<TransactionService> log)
        : AbstractService<TransactionService>(work, map, log), ITransactionService
    {
        #region AddAsync
        public async Task<long> AddAsync(TransactionModel entity)
        {
            try
            {
                if (entity is null || entity.Date >= DateTime.Now)
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                if (await work.CategoryOfTransactionRepository.GetByIdAsync(entity.CategoryId) is null)
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                if (await work.ChannelRepository.GetByIdAsync(entity.ChannelId) is null)
                {
                    throw new OptioGeneralException(ErrorKeys.InternalServerError);
                }
                if (await work.MerchantRepository.GetByIdAsync(entity.MerchantId) is null)
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                if (await work.CurrencyRepository.GetByIdAsync(entity.CurrencyNameId) is null)
                {
                    throw new OptioGeneralException(ErrorKeys.NotFound);
                }
                var mapped = mapper.Map<Transaction>(entity);
                if (mapped is null) throw new OptioGeneralException(ErrorKeys.Mapped);
                var res = await work.TransactionRepository.AddAsync(mapped);
                await work.CheckAndCommitAsync();
                return res;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }

        #endregion

        #region GetAllActiveAsync
        public async Task<IEnumerable<TransactionModel>> GetAllActiveAsync(TransactionModel identify)
        {
            try
            {
                var res = await work.TransactionRepository.GetAllActiveAsync();

                if (res is null) return new List<TransactionModel>();
                var mapped = mapper.Map<IEnumerable<TransactionModel>>(res);
                return mapped ?? throw new OptioGeneralException(ErrorKeys.NotFound);
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<TransactionModel>> GetAllAsync(TransactionModel identify)
        {
            try
            {
                var res = await work.TransactionRepository.GetAllWithDetailsAsync();

                if (res is null) return new List<TransactionModel>();
                var mapped = mapper.Map<IEnumerable<TransactionModel>>(res);
                return mapped ?? throw new OptioGeneralException($"{nameof(GetAllAsync)}");
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<TransactionModel> GetByIdAsync(long id, TransactionModel identify)
        {
            try
            {
                var res = await work.TransactionRepository.GetByIdAsync(id)
                          ?? throw new ItemNotFoundException(ErrorKeys.NotFound);
                var mapped = mapper.Map<TransactionModel>(res)
                             ?? throw new ItemNotFoundException(ErrorKeys.NotFound);
                return mapped;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(long id, TransactionModel identity)
        {
            try
            {
                var transaction = await work.TransactionRepository.GetByIdAsync(id);
                if (transaction is null) return false;
                var mapped = mapper.Map<Transaction>(transaction);
                if (mapped is null) return false;
                var res = await work.TransactionRepository.RemoveAsync(mapped);
                await work.CheckAndCommitAsync();
                return res;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(long id, TransactionModel identify)
        {
            try
            {
                var res = await work.TransactionRepository.SoftDeleteAsync(id);
                await work.CheckAndCommitAsync();
                return res;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(long id, TransactionModel entity)
        {
            try
            {
                if (entity is null) throw new ResourceNotFoundException(ErrorKeys.NotFound);
                var mapped = mapper.Map<Transaction>(entity)
                             ?? throw new ResourceNotFoundException(ErrorKeys.NotFound);
                var res = await work.TransactionRepository.UpdateAsync(id, mapped);
                await work.CheckAndCommitAsync();
                return res;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion
    }
}
