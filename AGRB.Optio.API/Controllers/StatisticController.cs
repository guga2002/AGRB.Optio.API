using Microsoft.AspNetCore.Mvc;
using AGRB.Optio.Application.Responses;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.ResponseModels;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Application.Interfaces.StatisticInterfaces;
using AGRB.Optio.Application.StaticFiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace RGBA.Optio.UI.Controllers
{
    /// <summary>
    /// Controller for Statistic Related Actions
    /// </summary>
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StatisticController: ControllerBase
    {
        private readonly IStatisticTransactionRelatedService transactionRelatedStatistic;
        private readonly IStatisticMerchantRelatedService merchantRelatedStatistic;
        /// Initializes a new instance of the <see cref="StatisticController"/> class.
        /// <param name="transactionRelatedStatistic">The TransactionrelateStatistic service.</param>
        /// <param name="merchantRelatedStatistic">The merchant Relate service.</param>
        public StatisticController(IStatisticTransactionRelatedService transactionRelatedStatistic, IStatisticMerchantRelatedService merchantRelatedStatistic)
        {
            this.transactionRelatedStatistic = transactionRelatedStatistic;
            this.merchantRelatedStatistic = merchantRelatedStatistic;
        }

        /// <summary>
        ///Get Most Popular category from transactions V2.0
        /// </summary>
        /// <returns>A response containing a lsit of CategoryResponseModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize User**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<CategoryResponseModel>>> MostPopularCategory(DateRangeRequestModel date)
        {
            var result = await transactionRelatedStatistic.GetMostPopularCategoryAsync(date.Start, date.End);
            return !result.Any()
                ? Response<IEnumerable<CategoryResponseModel>>.Error(ErrorKeys.NotFound)
                : Response<IEnumerable<CategoryResponseModel>>.Ok(result);
        }


        /// <summary>
        ///Get Transaction Quantity With Date from transactions V2.0
        /// </summary>
        /// <returns>A response containing a lsit of TransactionQuantitiesWithDateModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize User**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<TransactionQuantitiesWithDateModel>>> TransactionQuantityWithDate([FromBody] DateRangeRequestModel date)
        {
            var result = await transactionRelatedStatistic.GetTransactionQuantityWithDateAsync(date.Start, date.End);
            return !result.Any()
                ? Response<IEnumerable<TransactionQuantitiesWithDateModel>>.Error(ErrorKeys.BadRequest)
                : Response<IEnumerable<TransactionQuantitiesWithDateModel>>.Ok(result);
        }

        /// <summary>
        ///Get All Transaction Between Date from transactions V2.0
        /// </summary>
        /// <returns>A response containing a lsit of TransactionModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize User**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<TransactionModel>>> AllTransactionBetweenDate([FromBody] DateRangeRequestModel date)
        {
            var result = await transactionRelatedStatistic.GetAllTransactionBetweenDate(date.Start, date.End);
            return !result.Any()
                ? Response<IEnumerable<TransactionModel>>.Error(ErrorKeys.BadRequest)
                : Response<IEnumerable<TransactionModel>>.Ok(result);
        }


        /// <summary>
        ///Get Most Popular Channel  from transactions V2.0
        /// </summary>
        /// <returns>A response containing a lsit of ChannelResponseModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize User**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<ChannelResponseModel>>> MostPopularChannel([FromBody] DateRangeRequestModel date)
        {
            var result = await merchantRelatedStatistic.GetMostPopularChannelAsync(date.Start, date.End);
            return !result.Any()
                ? Response<IEnumerable<ChannelResponseModel>>.Error(ErrorKeys.BadRequest)
                : Response<IEnumerable<ChannelResponseModel>>.Ok(result);
        }

        /// <summary>
        ///Get Most Popular Location from transactions V2.0
        /// </summary>
        /// <returns>A response containing a lsit of LocationResponseModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize User**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<LocationResponseModel>>> MostPopularLocation([FromBody] DateRangeRequestModel date)
        {
            var result = await merchantRelatedStatistic.GetMostPopularLocationAsync(date.Start, date.End);
            return !result.Any()
                ? Response<IEnumerable<LocationResponseModel>>.Error(ErrorKeys.BadRequest)
                : Response<IEnumerable<LocationResponseModel>>.Ok(result);
        }

        /// <summary>
        ///Get Transactions with data range V2.0
        /// </summary>
        /// <returns>A response containing a lsit of MerchantResponseModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize User**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<MerchantResponseModel>>> MostPopularMerchants([FromBody] DateRangeRequestModel date)
        {
            var result = await merchantRelatedStatistic.GetMostPopularMerchantsAsync(date.Start, date.End);
            return !result.Any()
                ? Response<IEnumerable<MerchantResponseModel>>.Error(ErrorKeys.BadRequest)
                : Response<IEnumerable<MerchantResponseModel>>.Ok(result);
        }
    }
}
