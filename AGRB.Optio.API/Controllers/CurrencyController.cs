using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyController(ICurrencyRelatedService se, ILogger<CurrencyController> log) : ControllerBase 
    {
        [HttpPost]
        [Route(nameof(AddCurrency))]
        public async Task<IActionResult> AddCurrency([FromBody]CurrencyModel entity)
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
                    return Ok(entity);
                }
                return BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(AddExchangeRate))] 
        public async  Task<IActionResult> AddExchangeRate([FromBody]ExchangeRateModel entity)
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
                    return Ok(entity.DateOfExchangeRate.ToShortTimeString());
                }
                return BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(GetAllActiveCurrencyAsync))]
        public async Task<IActionResult> GetAllActiveCurrencyAsync()
        {
            try
            {
                var res =await se.GetAllActiveAsync(new CurrencyModel() { CurrencyCode="Undefined",NameOfCurrency="Undefined"});
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(AllActiveExchangeRate))]
        public async Task<IActionResult> AllActiveExchangeRate()
        {
            try
            {
                var res = await se.GetAllActiveAsync(new ExchangeRateModel() { DateOfExchangeRate=DateTime.Now,ExchangeRate=0,CurrencyId=0});
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(GetAllCurrencyAsync))]
        public async Task<IActionResult> GetAllCurrencyAsync()
        {
            try
            {
                var res = await se.GetAllAsync(new CurrencyModel() {CurrencyCode="Undefined",NameOfCurrency="Undefined"});
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route(nameof(GetAllExchangeRateAsync))]
        public async Task<IActionResult> GetAllExchangeRateAsync()
        {
            try
            {
                var res = await se.GetAllAsync(new ExchangeRateModel() { CurrencyId = 0, ExchangeRate = 0, DateOfExchangeRate = DateTime.Now });
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet()]
        [Route("Currency/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var res = await se.GetByIdAsync(id, new CurrencyModel() { CurrencyCode = "Undefined",NameOfCurrency="Undefined"});
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route("ExchangeRate/{id:long}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]long id)
        {
            try
            {
                var res = await se.GetByIdAsync(id, new ExchangeRateModel() {CurrencyId=0,ExchangeRate=0,DateOfExchangeRate=DateTime.Now});
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpDelete]
        [Route("Currency/{id:int}")]
        public async Task<IActionResult> RemoveCurrencyAsync([FromRoute]int id )
        {
            try
            {
                var rek = await se.RemoveAsync(id, new CurrencyModel() { CurrencyCode = "Undefined", NameOfCurrency = "Undefined" });
                if (rek)
                {
                    return Ok(rek);
                }
                return BadRequest(rek);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpDelete]
        [Route("ExchangeRate/{id:long}")]
        public  async Task<IActionResult> RemoveExchangeRateAsync([FromRoute] long id)
        {
            try
            {
                var res = await se.RemoveAsync(id, new ExchangeRateModel());
                if (res)
                {
                    return Ok(res);
                }
                return BadRequest(id);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPatch]
        [Route("Currency/{id:int}/[action]")]
        public async Task<IActionResult> SoftDelete([FromRoute]int id)
        {
            try
            {
                var res = await se.SoftDeleteAsync(id,new CurrencyModel() { CurrencyCode="undefined",NameOfCurrency="undefined"});
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPatch]
        [Route("ExchangeRate/{id:long}/[action]")]
        public async Task<IActionResult> SoftDelete([FromRoute] long id)
        {
            try
            {
                var res = await se.SoftDeleteAsync(id, new ExchangeRateModel() {CurrencyId=0,ExchangeRate=0,DateOfExchangeRate=DateTime.Now});
                if (res)
                {
                    return Ok(res);
                }

                return BadRequest(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPut]
        [Route("Currency/{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id,[FromBody]CurrencyModel entity)
        {
            try
            {
                var res = await se.UpdateAsync(id,entity);
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

        [HttpPut]
        [Route("ExchangeRate/{id:long}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long id,[FromBody]ExchangeRateModel mod)
        {
            try
            {
                var res = await se.UpdateAsync(id,mod);
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }

    }
}

