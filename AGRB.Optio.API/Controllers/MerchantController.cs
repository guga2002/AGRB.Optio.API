using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Responses;
using AGRB.Optio.Application.StaticFiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace RGBA.Optio.UI.Controllers
{
    /// <summary>
    /// Controller for Merchant Related Actions
    /// </summary>
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MerchantController: ControllerBase
    {

        private readonly IMerchantRelatedService se;
        private readonly IMemoryCache cashMemoryCache;

        /// Initializes a new instance of the <see cref="MerchantController"/> class.
        /// <param name="se">The Merchant service.</param>
        /// <param name="cashMemoryCache">The memory Cashing service.</param>
        public MerchantController(IMerchantRelatedService se, IMemoryCache cashMemoryCache)
        {
            this.se = se;
            this.cashMemoryCache = cashMemoryCache;

        }

        /// <summary>
        ///Get All Merchants from DB V2.0
        /// </summary>
        /// <returns>A response containing a lsit of merchants </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize user** cashing:**Actived**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<MerchantModel>>> GetAllMerchants()
        {
            const string cacheKey = "GetAllMerchantsKey";

            if (cashMemoryCache.TryGetValue(cacheKey, out IEnumerable<MerchantModel>? cachedData))
            {
                if (cachedData != null) return Response<IEnumerable<MerchantModel>>.Ok(cachedData);
            }
            else
            {
                var res = await se.GetAllAsync(new MerchantModel() { Name = DefaultText.NoText });
                if (!res.Any())
                {
                    return Response<IEnumerable<MerchantModel>>.Error(ErrorKeys.BadRequest);
                }

                cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                return Response<IEnumerable<MerchantModel>>.Ok(res);
            }
            return Response<IEnumerable<MerchantModel>>.Error(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Asign Location to merchant V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize user**
        /// </remarks>
        [HttpPost]
        [MapToApiVersion("2.0")]
        [Route("Merchant/{merchantId:long}/Location/{locationId:long}")]
        public async Task<Response<bool>> AssignLocationToMerchant([FromRoute] long merchantId, [FromRoute] long locationId)
        {
            var res = await se.AssignLocationToMerchant(merchantId, locationId);
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.NotFound);
        }

        /// <summary>
        ///Get All active Merchants from DB V1.0
        /// </summary>
        /// <returns>A response containing a lsit of merchants </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize user** cashing:**Actived**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("1.0")]
        public async Task<Response<IEnumerable<MerchantModel>>> AllActiveMerchant()
        {
            const string cacheKey = "GetAllActiveMerchantsKey";

            if (cashMemoryCache.TryGetValue(cacheKey, out IEnumerable<MerchantModel>? cachedData))
            {
                if (cachedData != null) return Response<IEnumerable<MerchantModel>>.Ok(cachedData);
            }
            else
            {
                var res = await se.GetAllActiveAsync(new MerchantModel() { Name = DefaultText.NoText });
                if (!res.Any())
                {
                    return Response<IEnumerable<MerchantModel>>.Error(ErrorKeys.NotFound);
                }
                cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                return Response<IEnumerable<MerchantModel>>.Ok(res);
            }
            return Response<IEnumerable<MerchantModel>>.Error(ErrorKeys.InternalServerError);
        }

        /// <summary>
        ///Get merchant By Id V2.0
        /// </summary>
        /// <returns>A response containing Merchant Model</returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,authorize user** Cashing:**Actived**
        /// </remarks>
        [HttpGet]
        [Route("Merchant/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<MerchantModel>> GetMerchant([FromRoute] long id)
        {
            var cacheKey = $"GetValueById{id}";

            if (cashMemoryCache.TryGetValue(cacheKey, out MerchantModel? value))
            {
                if (value != null) return Response<MerchantModel>.Ok(value);
            }
            else
            {
                var res = await se.GetByIdAsync(id, new MerchantModel() { Name = DefaultText.NoText });
                if (res is null)
                {
                    return Response<MerchantModel>.Error(ErrorKeys.NotFound);
                }

                cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                return Response<MerchantModel>.Ok(res);
            }

            return Response<MerchantModel>.Error(ErrorKeys.InternalServerError);
        }


        /// <summary>
        ///add merchant to DB V2.0
        /// </summary>
        /// <returns>A response containing long </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<long>> InsertMerchant([FromBody] MerchantModel value)
        {
            if (!ModelState.IsValid || value is null) return Response<long>.Error(ErrorKeys.BadRequest);
            var res = await se.AddAsync(value);
            return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.BadRequest);
        }


        /// <summary>
        ///Update merchants details V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPut]
        [Route("Merchant/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> UpdateMerchant([FromRoute] long id, [FromBody] MerchantModel value)
        {

            if (!ModelState.IsValid || value is null) return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await se.UpdateAsync(id, value);
            return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);
        }


        /// <summary>
        ///soft delete Merchant from Db V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> Delete([FromRoute] long id)
        {
            var res = await se.SoftDeleteAsync(id, new MerchantModel() { Name = DefaultText.NoText });
            return Response<bool>.Ok(res);
        }

        /// <summary>
        /// delete Merchant from Db V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpDelete]
        [Route("merchant/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> DeleteMerchant([FromRoute] long id)
        {
            var res = await se.RemoveAsync(id, new LocationModel() { LocationName = DefaultText.NoText });
            return Response<bool>.Ok(res);
        }


        /// <summary>
        ///Delete Location from DB V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpDelete]
        [Route("location/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> DeleteLocation([FromRoute] long id)
        {
            var res = await se.RemoveAsync(id, new LocationModel() { LocationName = DefaultText.NoText });
            return Response<bool>.Ok(res);
        }

        /// <summary>
        /// get all locations V2.0
        /// </summary>
        /// <returns>A response containing a list of Locations </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin, authorize user**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<LocationModel>>> GetLocation()
        {
            const string cacheKey = "GetLocation";
            if (cashMemoryCache.TryGetValue(cacheKey, out IEnumerable<LocationModel>? mod))
            {
                if (mod != null) return Response<IEnumerable<LocationModel>>.Ok(mod);
            }

            var res = await se.GetAllAsync(new LocationModel() { LocationName = DefaultText.NoText });
            if (!res.Any())
            {
                return Response<IEnumerable<LocationModel>>.Error(ErrorKeys.BadRequest);
            }
            cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
            return Response<IEnumerable<LocationModel>>.Ok(res);
        }


        /// <summary>
        ///Get all active locations V1.0
        /// </summary>
        /// <returns>A response containing a List of Active Locations </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin, authorized user**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("1.0")]
        public async Task<Response<IEnumerable<LocationModel>>> AllActiveLocation()
        {
            const string cashed = "GetAllActiveLocation";

            if (cashMemoryCache.TryGetValue(cashed, out IEnumerable<LocationModel>? loc))
            {
                if (loc != null) return Response<IEnumerable<LocationModel>>.Ok(loc);
            }

            var res = await se.GetAllActiveAsync(new LocationModel() { LocationName = DefaultText.NoText });
            if (!res.Any())
            {
                return Response<IEnumerable<LocationModel>>.Error(ErrorKeys.BadRequest);
            }
            cashMemoryCache.Set(cashed, res, TimeSpan.FromMinutes(15));
            return Response<IEnumerable<LocationModel>>.Ok(res);
        }


        /// <summary>
        /// Get Location by Id From DB V2.0
        /// </summary>
        /// <returns>A response containing List of LocationModel </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin,Authorize User**
        /// </remarks>
        [HttpGet]
        [Route("Location/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<LocationModel>> GetLocation([FromRoute] long id)
        {
            var cacheKey = $"getAllLocationById{id}";
            if (cashMemoryCache.TryGetValue(cacheKey, out LocationModel? mod))
            {
                if (mod != null) return Response<LocationModel>.Ok(mod);
            }

            var res = await se.GetByIdAsync(id, new LocationModel() { LocationName = DefaultText.NoText });
            if (res is null)
            {
                return Response<LocationModel>.Error(ErrorKeys.BadRequest);
            }
            cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
            return Response<LocationModel>.Ok(res);
        }

        /// <summary>
        /// Add Location to Database V2.0
        /// </summary>
        /// <returns>A response containing long </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [MapToApiVersion("2.0")]
        [HttpPost]
        [Route(nameof(InsertLocation))]
        public async Task<Response<long>> InsertLocation([FromBody] LocationModel value)
        {
            if (!ModelState.IsValid || value is null) return Response<long>.Error(ErrorKeys.BadRequest);
            var res = await se.AddAsync(value);
            return Response<long>.Ok(res);
        }


        /// <summary>
        ///update location Information V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **operator, manager,admin**
        /// </remarks>
        [HttpPut]
        [Route("Location/{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> Update([FromRoute] long id, [FromBody] LocationModel value)
        {
            if (!ModelState.IsValid || value is null) return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await se.UpdateAsync(id, value);
            return Response<bool>.Ok(res);
        }

        /// <summary>
        /// delete Location from Db V1
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **manager,admin**
        /// </remarks>
        [HttpPost]
        [Route("[action]/{id:long}")]
        [MapToApiVersion("1.0")]
        public async Task<Response<bool>> DeleteLocationSoft([FromRoute] long id)
        {
            var res = await se.SoftDeleteAsync(id, new LocationModel() { LocationName = DefaultText.NoText });
            return Response<bool>.Ok(res);
        }
    }
}
