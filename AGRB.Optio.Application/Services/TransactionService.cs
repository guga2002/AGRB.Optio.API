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
                if (await work.ChanellRepository.GetByIdAsync(entity.ChannelId) is null)
                {
                    throw new OptioGeneralException("CHanell  no exist while adding Transaction");
                }
                if (await work.MerchantRepository.GetByIdAsync(entity.MerchantId) is null)
                {
                    throw new OptioGeneralException("Merchant  no exist while adding Transaction");
                }
                if (await work.CurrencyRepository.GetByIdAsync(entity.CurencyNameId) is null)
                {
                    throw new OptioGeneralException("Such Currency no exist while adding Transaction");
                }
                var mapped = mapper.Map<Transaction>(entity);
                if (mapped is not null)
                {
                    var res=await work.TransactionRepository.AddAsync(mapped);
                    await work.CheckAndCommitAsync();
                    return res;
                }
                return -1;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message,exp.StackTrace);
                throw;
            }
        }

        #endregion

        #region GetAllActiveAsync
        public async Task<IEnumerable<TransactionModel>> GetAllActiveAsync(TransactionModel Identify)
        {
            try
            {
            var res = await work.TransactionRepository.GetAllActiveAsync();

                if(res is  not null)
                {
                    var mapped = mapper.Map<IEnumerable<TransactionModel>>(res);
                    if(mapped is not null)
                    {
                        return mapped;
                    }
                }
                return new List<TransactionModel>();
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<TransactionModel>> GetAllAsync(TransactionModel Identify)
        {
            try
            {
                var res = await work.TransactionRepository.GetAllWithDetailsAsync();

                if (res is not null)
                {
                    var mapped = mapper.Map<IEnumerable<TransactionModel>>(res);
                    if (mapped is not null)
                    {
                        return mapped;
                    }
                }
                return new List<TransactionModel>();
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<TransactionModel> GetByIdAsync(long id, TransactionModel Identify)
        {
            try
            {
                var res = await work.TransactionRepository.GetByIdAsync(id);
                if(res is not null)
                {
                    var mapped = mapper.Map<TransactionModel>(res);
                    if(mapped is not null)
                    {
                        return mapped;
                    }
                }
                throw new ItemNotFoundException(" No transavtion Exist");
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region RemoveAsync

        public async Task<bool> RemoveAsync(long Id, TransactionModel Identity)
        {
            try
            {
                var transac = await work.TransactionRepository.GetByIdAsync(Id);
                if (transac is not null)
                {
                    var mapped = mapper.Map<Transaction>(transac);
                    if (mapped is not null)
                    {
                        var res = await work.TransactionRepository.RemoveAsync(mapped);
                        await work.CheckAndCommitAsync();
                        return res;
                    }
                }
                return false;
            }
            catch (Exception exp)
            {
                logger.LogCritical(exp.Message, exp.StackTrace);
                throw;
            }
        }
        #endregion

        #region SoftDeleteAsync
        public async Task<bool> SoftDeleteAsync(long id, TransactionModel Identify)
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
                if (entity is not null)
                {
                    var mapped = mapper.Map<Transaction>(entity);
                    if (mapped is not null)
                    {
                        var res = await work.TransactionRepository.UpdateAsync(id,mapped);
                        await work.CheckAndCommitAsync();
                        return res;
                    }
                }
                throw new ResourceNotFoundException("No data exist  on this transaction in DB");
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
