using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

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
        public async Task<IActionResult> GetTransaction()
        {
            try
            {
                const string cacheKey = "GetAllTransaction";
                if (memoryCache.TryGetValue(cacheKey, out IEnumerable<TransactionModel>? value))
                {
                    return Ok(value);
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
                        return NotFound(ErrorKeys.NotFound);
                    }
                    memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> AllActiveTransaction()
        {
            try
            {
                const string cacheKey = "AllActiveTransaction";
                if (memoryCache.TryGetValue(cacheKey, out IEnumerable<TransactionModel>? value))
                {
                    return Ok(value);
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
                        return NotFound(ErrorKeys.NotFound);
                    }
                    memoryCache.Set(res, cacheKey, TimeSpan.FromMinutes(20));
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult>Get([FromRoute]long id)
        {
            try
            {
                var cacheKey = $"TransactionById: {id}";
                if (memoryCache.TryGetValue(cacheKey, out var value))
                {
                    return Ok(value);
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
                    return NotFound();
                }
                memoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                return Ok(res);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route(nameof(InsertTransaction))]
        public async Task<IActionResult> InsertTransaction([FromBody]TransactionModel model)
        {
            try
            {
                if (!ModelState.IsValid || model is null) return BadRequest(model);
                var res=await transactionService.AddAsync(model);
                if (res != -1)
                {
                    return Ok(res);
                }
                return StatusCode(405);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute]long id)
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
                return rek ? Ok(SuccessKeys.Success) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> DeleteSoft([FromRoute]long id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
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
                if (res)
                {
                    return Ok(res);
                }
                return StatusCode(405);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("[action]/{id:long}/[action]")]
        public async Task<IActionResult> UpdateTransaction([FromRoute]long id, [FromBody]TransactionModel transactionModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(transactionModel);
                var res = await transactionService.UpdateAsync(id, transactionModel);
                if (res)
                {
                    return Ok(res);
                }
                return StatusCode(405);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }
    }
}
