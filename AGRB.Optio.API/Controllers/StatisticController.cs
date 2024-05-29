using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Interfaces.StatisticInterfaces;
using RGBA.Optio.Domain.Models.RequestModels;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController(
        IStatisticTransactionRelatedService transactionRelatedStatistic,
        IStatisticMerchantRelatedService merchantRelatedStatistic)
        : ControllerBase
    {
        [HttpPost]
        [Route(nameof(GetMostPopularCategoryAsync))]
        public async Task<IActionResult> GetMostPopularCategoryAsync(DateRangeRequestModel date)
        {
            try
            {
               var result = await transactionRelatedStatistic.GetMostPopularCategoryAsync(date.Start,date.End);
                if(!result.Any())
                {
                    return NotFound(ErrorKeys.NotFound);
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(GetTransactionQuantityWithDateAsync))]
        public async Task<IActionResult> GetTransactionQuantityWithDateAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await transactionRelatedStatistic.GetTransactionQuantityWithDateAsync(date.Start, date.End);
                if (!result.Any())
                {
                    return NotFound(ErrorKeys.NotFound);
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(GetAllTransactionBetweenDate))]
        public async Task<IActionResult> GetAllTransactionBetweenDate([FromBody]DateRangeRequestModel date)
        {
            try
            {
                var result = await transactionRelatedStatistic.GetAllTransactionBetweenDate(date.Start, date.End);
                if (!result.Any())
                {
                    return NotFound(ErrorKeys.NotFound);
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(GetMostPopularChannelAsync))]
        public async Task<IActionResult> GetMostPopularChannelAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantRelatedStatistic.GetMostPopularChannelAsync(date.Start, date.End);
                if (!result.Any())
                {
                    return NotFound(ErrorKeys.NotFound);
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(GetMostPopularLocationAsync))]
        public async Task<IActionResult> GetMostPopularLocationAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantRelatedStatistic.GetMostPopularLocationAsync(date.Start, date.End);
                if (!result.Any())
                {
                    return NotFound(ErrorKeys.NotFound);
                }
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(GetMostPopularMerchantsAsync))]
        public async Task<IActionResult> GetMostPopularMerchantsAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantRelatedStatistic.GetMostPopularMerchantsAsync(date.Start, date.End);
                if (!result.Any())
                {
                    return NotFound(ErrorKeys.NotFound);
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
