using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Responses;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyController(ICurrencyRelatedService se, ILogger<CurrencyController> log) : ControllerBase 
    {
        [HttpPost]
        [Route(nameof(AddCurrency))]
        public async Task<Response<CurrencyModel>> AddCurrency([FromBody]CurrencyModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(entity.CurrencyCode);
                }
                var res = await se.AddAsync(entity);
                if(res!=-1)
                {
                    return Response<CurrencyModel>.Ok(entity);
                }
                return Response<CurrencyModel>.Error(ErrorKeys.BadRequest,nameof(entity));
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<CurrencyModel>.Error(exp.Message,exp.StackTrace);
            }
        }

        [HttpPost]
        [Route(nameof(AddExchangeRate))] 
        public async  Task<Response<ExchangeRateModel>> AddExchangeRate([FromBody]ExchangeRateModel entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(entity.DateOfExchangeRate.ToShortDateString());
                }
                var res = await se.AddAsync(entity);
                if (res != -1)
                {
                    return new Response<ExchangeRateModel>(true, entity);
                }
                return Response<ExchangeRateModel>.Error(ErrorKeys.BadRequest,nameof(entity));
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<ExchangeRateModel>.Error(exp.Message,value: exp.StackTrace);
            }
        }

        [HttpGet]
        [Route(nameof(GetAllActiveCurrencyAsync))]
        public async Task<Response<IEnumerable<CurrencyModel>>> GetAllActiveCurrencyAsync()
        {
            try
            {
                var res =await se.GetAllActiveAsync(new CurrencyModel() { CurrencyCode=DefaultText.NotDefined,NameOfCurrency=DefaultText.NoValue});
                return Response<IEnumerable<CurrencyModel>>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<IEnumerable<CurrencyModel>>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpGet]
        [Route(nameof(AllActiveExchangeRate))]
        public async Task<Response<IEnumerable<ExchangeRateModel>>> AllActiveExchangeRate()
        {
            try
            {
                var res = await se.GetAllActiveAsync(new ExchangeRateModel() { DateOfExchangeRate=DateTime.Now,ExchangeRate=0,CurrencyId=0});
                return Response<IEnumerable<ExchangeRateModel>>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<IEnumerable<ExchangeRateModel>>.Error(exp.Message,exp.StackTrace);
            }
        }

        [HttpGet]
        [Route(nameof(GetAllCurrencyAsync))]
        public async Task<Response<IEnumerable<CurrencyModel>>> GetAllCurrencyAsync()
        {
            try
            {
                var res = await se.GetAllAsync(new CurrencyModel() {CurrencyCode=DefaultText.NoText,NameOfCurrency=DefaultText.NotDefined});
                return Response<IEnumerable<CurrencyModel>>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<IEnumerable<CurrencyModel>>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpGet]
        [Route(nameof(GetAllExchangeRateAsync))]
        public async Task<Response<IEnumerable<ExchangeRateModel>>> GetAllExchangeRateAsync()
        {
            try
            {
                var res = await se.GetAllAsync(new ExchangeRateModel() { CurrencyId = 0, ExchangeRate = 0, DateOfExchangeRate = DateTime.Now });
                return Response<IEnumerable<ExchangeRateModel>>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<IEnumerable<ExchangeRateModel>>.Error(exp.Message,exp.StackTrace);
            }
        }

        [HttpGet()]
        [Route("Currency/{id:int}")]
        public async Task<Response<CurrencyModel>> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var res = await se.GetByIdAsync(id, new CurrencyModel() { CurrencyCode = DefaultText.NoValue, NameOfCurrency = DefaultText.NoValue });
                return Response<CurrencyModel>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<CurrencyModel>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpGet]
        [Route("ExchangeRate/{id:long}")]
        public async Task<Response<ExchangeRateModel>> GetByIdAsync([FromRoute]long id)
        {
            try
            {
                var res = await se.GetByIdAsync(id, new ExchangeRateModel() {CurrencyId=0,ExchangeRate=0,DateOfExchangeRate=DateTime.Now});
                return Response<ExchangeRateModel>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<ExchangeRateModel>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpDelete]
        [Route("Currency/{id:int}")]
        public async Task<Response<bool>> RemoveCurrencyAsync([FromRoute]int id )
        {
            try
            {
                var rek = await se.RemoveAsync(id, new CurrencyModel() { CurrencyCode = DefaultText.NoText, NameOfCurrency = DefaultText.NotDefined });
                if (rek)
                {
                    return Response<bool>.Ok(rek);
                }
                return Response<bool>.Error(ErrorKeys.NotFound);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message,exp.StackTrace);
            }
        }

        [HttpDelete]
        [Route("ExchangeRate/{id:long}")]
        public  async Task<Response<bool>> RemoveExchangeRateAsync([FromRoute] long id)
        {
            try
            {
                var res = await se.RemoveAsync(id, new ExchangeRateModel());
                if (res)
                {
                    return Response<bool>.Ok(res);
                }
                return Response<bool>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpPatch]
        [Route("Currency/{id:int}/[action]")]
        public async Task<Response<bool>> SoftDelete([FromRoute]int id)
        {
            try
            {
                var res = await se.SoftDeleteAsync(id,new CurrencyModel() { CurrencyCode=DefaultText.NoText,NameOfCurrency=DefaultText.NoText});
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message,exp.StackTrace);
            }
        }

        [HttpPatch]
        [Route("ExchangeRate/{id:long}/[action]")]
        public async Task<Response<bool>> SoftDelete([FromRoute] long id)
        {
            try
            {
                var res = await se.SoftDeleteAsync(id, new ExchangeRateModel() {CurrencyId=0,ExchangeRate=0,DateOfExchangeRate=DateTime.Now});
                if (res)
                {
                    return Response<bool>.Ok(res);
                }

                return Response<bool>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message,exp.StackTrace) ;
            }
        }

        [HttpPut]
        [Route("Currency/{id:int}")]
        public async Task<Response<bool>> UpdateAsync([FromRoute] int id,[FromBody]CurrencyModel entity)
        {
            try
            {
                var res = await se.UpdateAsync(id,entity);
                return Response<bool>.Ok (res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error (exp.Message,exp.StackTrace) ;
            }
        }

        [HttpPut]
        [Route("ExchangeRate/{id:long}")]
        public async Task<Response<bool>> UpdateAsync([FromRoute] long id,[FromBody]ExchangeRateModel mod)
        {
            try
            {
                var res = await se.UpdateAsync(id,mod);
                return Response<bool>.Ok ((res) ? res : false);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message,exp.StackTrace,exp.Source);
            }
        }

    }
}

