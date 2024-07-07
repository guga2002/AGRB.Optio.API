using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AGRB.Optio.Domain.Custom_Exceptions;
using AGRB.Optio.Application.StaticFiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RGBA.Optio.UI.Controllers
{
    /// <summary>
    /// Controller for Currency and Exchange rates operations.
    /// </summary>
   
    [ApiController]
    [Authorize]
    [ApiVersion("1.0",Deprecated =true)]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CurrencyController: ControllerBase 
    {

        private readonly ICurrencyRelatedService se;

        /// Initializes a new instance of the <see cref="CurrencyController"/> class.
        /// <param name="se">The Currencyrelated service.</param>
        public CurrencyController(ICurrencyRelatedService se)
        {
            this.se = se;
        }
        /// <summary>
        /// Add currency to Db V2.0
        /// </summary>
        /// <returns>A response containing a Currency Model Which added.</returns>
        /// <remarks>
        ///  avalible for **Admin ,Manager**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<CurrencyModel>> Currency([FromBody] CurrencyModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(entity.CurrencyCode);
            }
            var res = await se.AddAsync(entity);
            if (res != -1)
            {
                return Response<CurrencyModel>.Ok(entity);
            }
            return Response<CurrencyModel>.Error(ErrorKeys.BadRequest, nameof(entity));
        }

        /// <summary>
        /// Add Ecxhange Rate to Db V2.0
        /// </summary>
        /// <returns>A response containing a ExchangeRate Model Which added.</returns>
        /// <remarks>
        /// avalible for **Admin ,Manager**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<ExchangeRateModel>> ExchangeRate([FromBody] ExchangeRateModel entity)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(entity.Date.ToShortDateString());
            }
            var res = await se.AddAsync(entity);
            if (res != -1)
            {
                return new Response<ExchangeRateModel>(true, entity);
            }
            return Response<ExchangeRateModel>.Error(ErrorKeys.BadRequest, nameof(entity));
        }

        /// <summary>
        /// Get All Active Currencies from DB V1.0
        /// </summary>
        /// <returns>A response containing a List of Currencies.</returns>
        /// <remarks>
        ///avalible for **Authorize User**
        /// </remarks>
        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<CurrencyModel>>> AllActiveCurrencyV1()
        {
            var res = await se.GetAllActiveAsync(new CurrencyModel() { CurrencyCode = DefaultText.NotDefined, NameOfCurrency = DefaultText.NoValue });
            return Response<IEnumerable<CurrencyModel>>.Ok(res);
        }

        /// <summary>
        /// Get All Active Currencies from DB  V2.0
        /// </summary>
        /// <returns>A response containing a List of Currencies.</returns>
        /// <remarks>
        ///avalible for **Authorize User**
        /// </remarks>
        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> AllActiveCurrencyV2()
        {
            await Task.Delay(10);
            return Ok("Version 2 is under development");
        }

        /// <summary>
        /// Get all Active exchangeRates from DB V1.0
        /// </summary>
        /// <returns>A response containing a List of ExchangeRates</returns>
        /// <remarks>
        ///avalible for **Authorize Users**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("1.0")]
        public async Task<Response<IEnumerable<ExchangeRateModel>>> AllActiveExchangeRate()
        {

            var res = await se.GetAllActiveAsync(new ExchangeRateModel() { Date = DateTime.Now, Rate = 0, CurrencyId = 0 });
            return Response<IEnumerable<ExchangeRateModel>>.Ok(res);
        }


        /// <summary>
        /// Get All  Currencies from DB V2.0
        /// </summary>
        /// <returns>A response containing a List of Currencies.</returns>
        /// <remarks>
        ///  avalible for **Authorize User**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<CurrencyModel>>> AllCurrencyAsync()
        {
            var res = await se.GetAllAsync(new CurrencyModel() { CurrencyCode = DefaultText.NoText, NameOfCurrency = DefaultText.NotDefined });
            return Response<IEnumerable<CurrencyModel>>.Ok(res);
        }


        /// <summary>
        /// Get all exchangeRates from DB V2.0
        /// </summary>
        /// <returns>A response containing a List of ExchangeRates</returns>
        /// <remarks>
        /// avalible for **Authorize Users**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<ExchangeRateModel>>> AllExchangeRateAsync()
        {
            var res = await se.GetAllAsync(new ExchangeRateModel() { CurrencyId = 0, Rate = 0, Date = DateTime.Now });
            return Response<IEnumerable<ExchangeRateModel>>.Ok(res);
        }

        /// <summary>
        /// Get Currency By It Id V2.0
        /// </summary>
        /// <returns>A response containing a Currency Model</returns>
        /// <remarks>
        /// avalible for **Authorize Users**
        /// </remarks>
        [HttpGet()]
        [Route("currency/{id:int}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<CurrencyModel>> ByIdAsync([FromRoute] int id)
        {
            var res = await se.GetByIdAsync(id, new CurrencyModel() { CurrencyCode = DefaultText.NoValue, NameOfCurrency = DefaultText.NoValue });
            return Response<CurrencyModel>.Ok(res);
        }

        /// <summary>
        /// Get ExchangeRates By It Id V2.0
        /// </summary>
        /// <returns>A response containing a ExchangeRate Model</returns>
        /// <remarks>
        /// avalible for **Authorize Users**
        /// </remarks>
        [HttpGet]
        [Route("exchangerate/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<ExchangeRateModel>> ByIdAsync([FromRoute] long id)
        {
            var res = await se.GetByIdAsync(id, new ExchangeRateModel() { CurrencyId = 0, Rate = 0, Date = DateTime.Now });
            return Response<ExchangeRateModel>.Ok(res);
        }

        /// <summary>
        /// Delete Currency From DB V2.0
        /// </summary>
        /// <returns>A response containing boolean.</returns>
        /// <remarks>
        ///  avalible for **Admin ,Manager**
        /// </remarks>
        [HttpDelete]
        [Route("currency/{id:int}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> CurrencyAsync([FromRoute] int id)
        {
            var rek = await se.RemoveAsync(id, new CurrencyModel() { CurrencyCode = DefaultText.NoText, NameOfCurrency = DefaultText.NotDefined });
            if (rek)
            {
                return Response<bool>.Ok(rek);
            }
            return Response<bool>.Error(ErrorKeys.NotFound);
        }


        /// <summary>
        /// Delete Exchange Rate from Db V2.0
        /// </summary>
        /// <returns>A response containing boolean .</returns>
        /// <remarks>
        ///  avalible for **Admin,Manager**
        /// </remarks>
        [HttpDelete]
        [Route("exchangerate/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> ExchangeRateAsync([FromRoute] long id)
        {
            var res = await se.RemoveAsync(id, new ExchangeRateModel());
            if (res)
            {
                return Response<bool>.Ok(res);
            }
            return Response<bool>.Error(ErrorKeys.BadRequest);
        }


        /// <summary>
        /// Soft Delete Currency V2.0
        /// </summary>
        /// <returns>A response containing boolean.</returns>
        /// <remarks>
        ///  avalible for **Admin,Manager**
        /// </remarks>
        [HttpPatch]
        [Route("currency/{id:int}/[action]")]
        [MapToApiVersion("1.0")]
        public async Task<Response<bool>> SoftDelete([FromRoute] int id)
        {
            var res = await se.SoftDeleteAsync(id, new CurrencyModel() { CurrencyCode = DefaultText.NoText, NameOfCurrency = DefaultText.NoText });
            return Response<bool>.Ok(res);
        }

        /// <summary>
        /// Soft Delete ExchangeRate V1.0
        /// </summary>
        /// <returns>A response containing boolean</returns>
        /// <remarks>
        ///  avalible for **Admin,Manager**
        /// </remarks>
        [HttpPatch]
        [Route("exchangerate/{id:long}/[action]")]
        [MapToApiVersion("1.0")]
        public async Task<Response<bool>> SoftDelete([FromRoute] long id)
        {
            var res = await se.SoftDeleteAsync(id, new ExchangeRateModel() { CurrencyId = 0, Rate = 0, Date = DateTime.Now });
            if (res)
            {
                return Response<bool>.Ok(res);
            }

            return Response<bool>.Error(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Update Currency Details In DB V2.0
        /// </summary>
        /// <returns>A response containing bool.</returns>
        /// <remarks>
        ///  avalible for **Admin,Manager,Operator**
        /// </remarks>
        [HttpPut]
        [Route("currency/{id:int}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> UpdateAsync([FromRoute] int id, [FromBody] CurrencyModel entity)
        {
            var res = await se.UpdateAsync(id, entity);
            return Response<bool>.Ok(res);
        }

        /// <summary>
        /// Update exchange rate V2.0
        /// </summary>
        /// <returns>A response containing a List of Currencies.</returns>
        /// <remarks>
        ///  avalible for **Authorize User**
        /// </remarks>
        [HttpPut]
        [Route("exchangerate/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> UpdateAsync([FromRoute] long id, [FromBody] ExchangeRateModel mod)
        {
            var res = await se.UpdateAsync(id, mod);
            return Response<bool>.Ok((res) ? res : false);
        }

    }
}

