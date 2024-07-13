using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Responses;
using AGRB.Optio.Application.StaticFiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace RGBA.Optio.UI.Controllers
{
    /// <summary>
    /// Controller for Transaction Related Actions
    /// </summary>
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController: ControllerBase
    {

        private readonly ITransactionService transactionService;
        private readonly IMemoryCache memoryCache;

        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// <param name="transactionService">The Transaction service.</param>
        /// <param name="memoryCache">The memory cash service.</param>
        public TransactionController(ITransactionService transactionService, IMemoryCache memoryCache)
        {
            this.transactionService = transactionService;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        ///Get all  transactions V2.0
        /// </summary>
        /// <returns>A response containing a lsit of TransactionModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<TransactionModel>>> Transactions()
        {
            const string cacheKey = "GetAllTransaction";
            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<TransactionModel>? value))
            {
                if (value != null) return Response<IEnumerable<TransactionModel>>.Ok(value);
            }
            else
            {
                var res = await transactionService.GetAllAsync(new TransactionModel
                {
                    Amount = 0,
                    CategoryId = 0,
                    ChannelId = 0,
                    MerchantId = 0,
                    CurrencyNameId = 0,
                    Date = DateTime.Now,
                    EquivalentInGel = 0
                });

                if (!res.Any())
                {
                    return Response<IEnumerable<TransactionModel>>.Error(ErrorKeys.BadRequest);
                }
                memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                return Response<IEnumerable<TransactionModel>>.Ok(res);
            }
            return Response<IEnumerable<TransactionModel>>.Error(ErrorKeys.InternalServerError);
        }

        /// <summary>
        ///Get all active transactions V1.0
        /// </summary>
        /// <returns>A response containing a lsit of TransactionModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("1.0")]
        public async Task<Response<IEnumerable<TransactionModel>>> ActiveTransactions()
        {
            const string cacheKey = "AllActiveTransaction";
            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<TransactionModel>? value))
            {
                if (value != null) return Response<IEnumerable<TransactionModel>>.Ok(value);
            }
            else
            {
                var res = await transactionService.GetAllActiveAsync(new TransactionModel
                {
                    Amount = 0,
                    CategoryId = 0,
                    ChannelId = 0,
                    MerchantId = 0,
                    CurrencyNameId = 0,
                    Date = DateTime.Now,
                    EquivalentInGel = 0
                });
                if (res is null)
                {
                    return Response<IEnumerable<TransactionModel>>.Error(ErrorKeys.BadRequest);
                }
                memoryCache.Set(res, cacheKey, TimeSpan.FromMinutes(20));
                return Response<IEnumerable<TransactionModel>>.Ok(res);
            }

            return Response<IEnumerable<TransactionModel>>.Error(ErrorKeys.BadRequest);
        }

        /// <summary>
        ///Get transactions by id V2.0
        /// </summary>
        /// <returns>A response containing TransactionModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpGet]
        [Route("{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<TransactionModel>> Transaction([FromRoute] long id)
        {

            var cacheKey = $"TransactionById: {id}";
            if (memoryCache.TryGetValue(cacheKey, out TransactionModel? value))
            {
                if (value != null) return Response<TransactionModel>.Ok(value);
            }

            var res = await transactionService.GetByIdAsync(id, new TransactionModel
            {
                Amount = 0,
                CategoryId = 0,
                ChannelId = 0,
                MerchantId = 0,
                CurrencyNameId = 0,
                Date = DateTime.Now,
                EquivalentInGel = 0
            });
            if (res is null)
            {
                return Response<TransactionModel>.Error(ErrorKeys.BadRequest);
            }
            memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
            return Response<TransactionModel>.Ok(res);
        }


        /// <summary>
        /// Add new Transaction to db V2.0
        /// </summary>
        /// <returns>A response containing a long </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<long>> Insert([FromBody] TransactionModel model)
        {
            if (!ModelState.IsValid || model is null) return Response<long>.Error(ErrorKeys.BadRequest);
            var res = await transactionService.AddAsync(model);
            return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.NotFound);
        }

        /// <summary>
        /// Delete transaction from Db V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **manager,admin**
        /// </remarks>
        [HttpDelete]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> DeleteTransaction([FromRoute] long id)
        {

            var rek = await transactionService.RemoveAsync(id, new TransactionModel()
            {
                Amount = 0,
                Date = DateTime.Now,
                CategoryId = 0,
                ChannelId = 0,
                CurrencyNameId = 0,
                EquivalentInGel = 4,
                MerchantId = 0
            });
            return rek ? Response<bool>.Ok(rek) : Response<bool>.Error(ErrorKeys.NotFound);
        }

        /// <summary>
        /// soft Delete transaction from Db V1.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("1.0")]
        public async Task<Response<bool>> SoftDelete([FromRoute] long id)
        {
            if (!ModelState.IsValid) return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await transactionService.SoftDeleteAsync(id, new TransactionModel
            {
                Amount = 0,
                CategoryId = 0,
                ChannelId = 0,
                MerchantId = 0,
                CurrencyNameId = 0,
                Date = DateTime.Now,
                EquivalentInGel = 0
            });
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.NotFound);
        }

        /// <summary>
        /// Update transaction from Db V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **manager,admin**
        /// </remarks>
        [HttpPut]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> Transaction([FromRoute] long id, [FromBody] TransactionModel transactionModel)
        {
            if (!ModelState.IsValid) return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await transactionService.UpdateAsync(id, transactionModel);
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.NotFound);
        }
    }
}
