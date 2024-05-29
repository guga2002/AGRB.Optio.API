using AutoMapper;
using Microsoft.Extensions.Logging;
using Optio.Core.Entities;
using RGBA.Optio.Core.Interfaces;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Services
{
    public class TransactionService(IUniteOfWork work, IMapper map, ILogger<TransactionService> log)
        : AbstractService<TransactionService>(work, map, log), ITransactionService
    {
        #region AddAsync
        public async  Task<long> AddAsync(TransactionModel entity)
        {
            try
            {
                if (entity is null || entity.Date >= DateTime.Now)
                {
                    throw new OptioGeneralException("Exception while adding  Transaction");
                }
                if (await work.CategoryOfTransactionRepository.GetByIdAsync(entity.CategoryId) is null)
                {
                    throw new OptioGeneralException("Such Category no exist");
                }
                if (await work.ChannelRepository.GetByIdAsync(entity.ChannelId) is null)
                {
                    throw new OptioGeneralException("Channel  no exist while adding Transaction");
                }
                if (await work.MerchantRepository.GetByIdAsync(entity.MerchantId) is null)
                {
                    throw new OptioGeneralException("Merchant  no exist while adding Transaction");
                }
                if (await work.CurrencyRepository.GetByIdAsync(entity.CurrencyNameId) is null)
                {
                    throw new OptioGeneralException("Such Currency no exist while adding Transaction");
                }
                var mapped = mapper.Map<Transaction>(entity);
                if (mapped is null) return -1;
                var res=await work.TransactionRepository.AddAsync(mapped);
                await work.CheckAndCommitAsync();
                return res;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message,exp.StackTrace);
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
                return mapped ?? new List<TransactionModel>();
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
                return mapped ?? new List<TransactionModel>();
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
                          ?? throw new ItemNotFoundException(" No transaction Exist");
                var mapped = mapper.Map<TransactionModel>(res)
                             ?? throw new ItemNotFoundException(" No transaction Exist");
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
                if (entity is null) throw new ResourceNotFoundException("No data exist  on this transaction in DB");
                var mapped = mapper.Map<Transaction>(entity) 
                             ?? throw new ResourceNotFoundException("No data exist  on this transaction in DB");
                var res = await work.TransactionRepository.UpdateAsync(id,mapped);
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
