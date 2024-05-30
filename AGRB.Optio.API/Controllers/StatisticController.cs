using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Interfaces.StatisticInterfaces;
using RGBA.Optio.Domain.Models.RequestModels;
using RGBA.Optio.Domain.Models.ResponseModels;
using RGBA.Optio.Domain.Responses;
using RGBA.Optio.Domain.Models;

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
        public async Task<Response<IEnumerable<CategoryResponseModel>>> GetMostPopularCategoryAsync(DateRangeRequestModel date)
        {
            try
            {
               var result = await transactionRelatedStatistic.GetMostPopularCategoryAsync(date.Start,date.End);
               return !result.Any()
                   ? Response<IEnumerable<CategoryResponseModel>>.Error(ErrorKeys.NotFound)
                   : Response<IEnumerable<CategoryResponseModel>>.Ok(result);
            }
            catch (Exception exp)
            {
                return Response<IEnumerable<CategoryResponseModel>>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpPost]
        [Route(nameof(GetTransactionQuantityWithDateAsync))]
        public async Task<Response<IEnumerable<TransactionQuantitiesWithDateModel>>> GetTransactionQuantityWithDateAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await transactionRelatedStatistic.GetTransactionQuantityWithDateAsync(date.Start, date.End);
                return !result.Any()
                    ? Response<IEnumerable<TransactionQuantitiesWithDateModel>>.Error(ErrorKeys.BadRequest)
                    : Response<IEnumerable<TransactionQuantitiesWithDateModel>>.Ok(result);
            }
            catch (Exception exp)
            {
                return Response<IEnumerable<TransactionQuantitiesWithDateModel>>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpPost]
        [Route(nameof(GetAllTransactionBetweenDate))]
        public async Task<Response<IEnumerable<TransactionModel>>> GetAllTransactionBetweenDate([FromBody]DateRangeRequestModel date)
        {
            try
            {
                var result = await transactionRelatedStatistic.GetAllTransactionBetweenDate(date.Start, date.End);
                return !result.Any()
                    ? Response<IEnumerable<TransactionModel>>.Error(ErrorKeys.BadRequest)
                    : Response<IEnumerable<TransactionModel>>.Ok(result);
            }
            catch (Exception exp)
            {
                return Response<IEnumerable<TransactionModel>>.Error(exp.Message, exp.StackTrace);
            }
        }


        [HttpPost]
        [Route(nameof(GetMostPopularChannelAsync))]
        public async Task<Response<IEnumerable<ChannelResponseModel>>> GetMostPopularChannelAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantRelatedStatistic.GetMostPopularChannelAsync(date.Start, date.End);
                return !result.Any()
                    ? Response<IEnumerable<ChannelResponseModel>>.Error(ErrorKeys.BadRequest)
                    : Response<IEnumerable<ChannelResponseModel>>.Ok(result);
            }
            catch (Exception exp)
            {
                return Response<IEnumerable<ChannelResponseModel>>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpPost]
        [Route(nameof(GetMostPopularLocationAsync))]
        public async Task<Response<IEnumerable<LocationResponseModel>>> GetMostPopularLocationAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantRelatedStatistic.GetMostPopularLocationAsync(date.Start, date.End);
                return !result.Any()
                    ? Response<IEnumerable<LocationResponseModel>>.Error(ErrorKeys.BadRequest)
                    : Response<IEnumerable<LocationResponseModel>>.Ok(result);
            }
            catch (Exception exp)
            {
                return Response<IEnumerable<LocationResponseModel>>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpPost]
        [Route(nameof(GetMostPopularMerchantsAsync))]
        public async Task<Response<IEnumerable<MerchantResponseModel>>> GetMostPopularMerchantsAsync([FromBody] DateRangeRequestModel date)
        {
            try
            {
                var result = await merchantRelatedStatistic.GetMostPopularMerchantsAsync(date.Start, date.End);
                return !result.Any()
                    ? Response<IEnumerable<MerchantResponseModel>>.Error(ErrorKeys.BadRequest)
                    : Response<IEnumerable<MerchantResponseModel>>.Ok(result);
            }
            catch (Exception exp)
            {
                return Response<IEnumerable<MerchantResponseModel>>.Error(exp.Message,exp.StackTrace);
            }
        }
    }
}
