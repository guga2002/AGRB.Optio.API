using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Interfaces.StatisticInterfaces;
using RGBA.Optio.Domain.Models.RequestModels;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticTransactionRelatedService transactionrelatedstatistic;
        private readonly IStatisticMerchantRelatedService merchantrelatedstatistic;

        public StatisticController(IStatisticTransactionRelatedService transactionrelatedstatistic,
            IStatisticMerchantRelatedService merchantrelatedstatistic)
        {
            this.transactionrelatedstatistic = transactionrelatedstatistic;
            this.merchantrelatedstatistic = merchantrelatedstatistic;
        }

        [HttpPost]
        [Route("MostPopularCategory")]
        public async Task<IActionResult> GetMostPopularCategoryAsync(DateRangeRequestModel date)
        {
            try
            {
               var result = await transactionrelatedstatistic.GetMostPopularCategoryAsync(date.start,date.end);
                if(!result.Any())
                {
                    return NotFound("No data exist on this range");
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("TransactionQuantityWithDate")]
        public async Task<IActionResult> GetTransactionQuantityWithDateAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await transactionrelatedstatistic.GetTransactionQuantityWithDateAsync(date.start, date.end);
                if (!result.Any())
                {
                    return NotFound("No data exist on this range");
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("AllTransactionBetweenDate")]
        public async Task<IActionResult> GetAllTransactionBetweenDate([FromBody]DateRangeRequestModel date)
        {
            try
            {
                var result = await transactionrelatedstatistic.GetAllTransactionBetweenDate(date.start, date.end);
                if (!result.Any())
                {
                    return NotFound("No data exist on this range");
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("MostPopularChannel")]
        public async Task<IActionResult> GetMostPopularChannelAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantrelatedstatistic.GetMostPopularChannelAsync(date.start, date.end);
                if (!result.Any())
                {
                    return NotFound("No data exist on this range");
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("MostPopularLocation")]
        public async Task<IActionResult> GetMostPopularLocationAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantrelatedstatistic.GetMostPopularLocationAsync(date.start, date.end);
                if (!result.Any())
                {
                    return NotFound("No data exist on this range");
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("MostPopularMerchants")]
        public async Task<IActionResult> GetMostPopularMerchantsAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantrelatedstatistic.GetMostPopularMerchantsAsync(date.start, date.end);
                if (!result.Any())
                {
                    return NotFound("No data exist on this range");
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }
    }
}
