using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Responses;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(
        ITransactionService transactionService,
        ILogger<TransactionController> logger,
        IMemoryCache memoryCache)
        : ControllerBase
    {
        [HttpGet]
        [Route(nameof(GetTransaction))]
        public async Task<Response<IEnumerable<TransactionModel>>> GetTransaction()
        {
            try
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
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return Response<IEnumerable<TransactionModel>>.Error(ex.Message, ex.StackTrace);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<TransactionModel>>> AllActiveTransaction()
        {
            try
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
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return Response<IEnumerable<TransactionModel>>.Error(ex.Message, ex.StackTrace);
            }
        }


        [HttpGet]
        [Route("{id:long}")]
        public async Task<Response<TransactionModel>>Get([FromRoute]long id)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return Response<TransactionModel>.Error(ex.Message,ex.StackTrace);
            }
        }


        [HttpPost]
        [Route(nameof(InsertTransaction))]
        public async Task<Response<long>> InsertTransaction([FromBody]TransactionModel model)
        {
            try
            {
                if (!ModelState.IsValid || model is null) return Response<long>.Error(ErrorKeys.BadRequest);
                var res=await transactionService.AddAsync(model);
                return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.NotFound);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return Response<long>.Error(ex.Message, ex.StackTrace);
            }
        }


        [HttpDelete]
        [Route("[action]/{id:long}")]
        public async Task<Response<bool>> DeleteTransaction([FromRoute]long id)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return Response<bool>.Error(ex.Message, ex.StackTrace);
            }
        }


        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<Response<bool>> DeleteSoft([FromRoute]long id)
        {
            try
            {
                if (!ModelState.IsValid) return Response<bool>.Error(ErrorKeys.BadRequest);
                var res =await transactionService.SoftDeleteAsync(id, new TransactionModel
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
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return Response<bool>.Error(ex.Message, ex.StackTrace);
            }
        }


        [HttpPut]
        [Route("[action]/{id:long}/[action]")]
        public async Task<Response<bool>> UpdateTransaction([FromRoute]long id, [FromBody]TransactionModel transactionModel)
        {
            try
            {
                if (!ModelState.IsValid) return Response<bool>.Error(ErrorKeys.BadRequest);
                var res = await transactionService.UpdateAsync(id, transactionModel);
                return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.NotFound);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return Response<bool>.Error(ex.Message, ex.StackTrace);
            }
        }
    }
}
