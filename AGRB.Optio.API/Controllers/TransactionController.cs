using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly ILogger<TransactionController> logger;
        private readonly IMemoryCache memoryCache;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger, IMemoryCache memoryCache)
        {
            this.transactionService = transactionService;
            this.logger = logger;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string cachekey = "GetAllTransaction";
                if (memoryCache.TryGetValue(cachekey, out IEnumerable<TransactionModel>? value))
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
                        CurencyNameId = 0,
                        Date = DateTime.Now,
                        EquivalentInGel = 0
                    });

                    if (!res.Any())
                    {
                        return NotFound("data no exist");
                    }
                    memoryCache.Set(cachekey, res, TimeSpan.FromMinutes(20));
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
        public async Task<IActionResult> AllActive()
        {
            try
            {
                string cacheKey = "AllActiveTransaction";
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
                        CurencyNameId = 0,
                        Date = DateTime.Now,
                        EquivalentInGel = 0
                    });
                    if (res is null)
                    {
                        return NotFound();
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
                string cachekey = $"TransactionById: {id}";
                if (memoryCache.TryGetValue(cachekey, out var value))
                {
                    return Ok(value);
                }
                else
                {
                    var res = await transactionService.GetByIdAsync(id, new TransactionModel
                    {
                        Amount = 0,
                        CategoryId = 0,
                        ChannelId = 0,
                        MerchantId = 0,
                        CurencyNameId = 0,
                        Date = DateTime.Now,
                        EquivalentInGel = 0
                    });
                    if (res is null)
                    {
                        return NotFound();
                    }
                    memoryCache.Set(cachekey, res, TimeSpan.FromMinutes(20));
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]TransactionModel model)
        {
            try
            {
                if (ModelState.IsValid && model is not null)
                {
                    var res=await transactionService.AddAsync(model);
                    if (res != -1)
                    {
                        return Ok(res);
                    }
                    return StatusCode(405);
                }
                return BadRequest(model);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("transaction/{id:long}")]
        public async Task<IActionResult> Delete([FromRoute]long id)
        {
            try
            {
                var rek = await transactionService.RemoveAsync(id, new TransactionModel()
                {
                    Amount = 0,
                    Date = DateTime.Now,
                    CategoryId = 0,
                    ChannelId = 0,
                    CurencyNameId = 0,
                    EquivalentInGel = 4,
                    MerchantId = 0
                });
                return rek == true ? Ok("succesfully deleted") : BadRequest("warumatebeli");
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
                if (ModelState.IsValid)
                {
                    var res =await transactionService.SoftDeleteAsync(id, new TransactionModel
                    {
                        Amount = 0,
                        CategoryId = 0,
                        ChannelId = 0,
                        MerchantId = 0,
                        CurencyNameId = 0,
                        Date = DateTime.Now,
                        EquivalentInGel = 0
                    });
                    if (res)
                    {
                        return Ok(res);
                    }
                    return StatusCode(405);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Transaction/{id:long}/[action]")]
        public async Task<IActionResult> Update([FromRoute]long id, [FromBody]TransactionModel transactionModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await transactionService.UpdateAsync(id, transactionModel);
                    if (res)
                    {
                        return Ok(res);
                    }
                    return StatusCode(405);
                }
                return BadRequest(transactionModel);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message, ex.StackTrace, DateTime.Now.ToShortTimeString());
                return BadRequest(ex.Message);
            }
        }
    }
}
