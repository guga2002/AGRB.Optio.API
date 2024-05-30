using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Responses;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController(
        IMerchantRelatedService se,
        ILogger<MerchantController> log,
        IMemoryCache cashMemoryCache)
        : ControllerBase
    {
        [HttpGet]
        [Route(nameof(GetAllMerchants))]
        public async Task<Response<IEnumerable<MerchantModel>>> GetAllMerchants()
        {
            try
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
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<IEnumerable<MerchantModel>>.Error(ErrorKeys.BadRequest, exp.Message, exp.StackTrace);
            }
        }
        

        [HttpPost]
        [Route("Merchant/{merchantId:long}/Location/{locationId:long}")]
        public async Task<Response<bool>> AssignLocationToMerchant([FromRoute]long merchantId,[FromRoute] long locationId)
        {
            try
            {
                var res = await se.AssignLocationToMerchant(merchantId, locationId);
                return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.NotFound);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<bool>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpGet]
        [Route(nameof(AllActiveMerchant))]
        public async Task<Response<IEnumerable<MerchantModel>>> AllActiveMerchant()
        {
            try
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
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<IEnumerable<MerchantModel>>.Error(exp.Message, exp.StackTrace);
            }
        }


        [HttpGet]
        [Route("Merchant/{id:long}")]
        public async Task<Response<MerchantModel>> GetMerchant([FromRoute] long id)
        {
            try
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
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<MerchantModel>.Error(exp.Message, exp.StackTrace);
            }
        }


        [HttpPost]
        [Route(nameof(InsertMerchant))]
        public async Task<Response<long>> InsertMerchant([FromBody] MerchantModel value)
        {
            try
            {
                if (!ModelState.IsValid || value is null) return Response<long>.Error(ErrorKeys.BadRequest);
                var res= await se.AddAsync(value);
                return res != -1 ? Response<long>.Ok(res) : Response<long>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
               return Response<long>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpPut]
        [Route("Merchant/{id:long}")]
        public async Task<Response<bool>> UpdateMerchant([FromRoute] long id, [FromBody] MerchantModel value)
        {
            try
            {
                if (!ModelState.IsValid || value is null) return Response<bool>.Error(ErrorKeys.BadRequest);
                var res = await se.UpdateAsync(id,value);
                return res ? Response<bool>.Ok(res) : Response<bool>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message, exp.StackTrace);
            }
        }


        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<Response<bool>>  Delete([FromRoute] long id)
        {
            try
            {
                var res = await se.SoftDeleteAsync(id, new MerchantModel() { Name = DefaultText.NoText });
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message, exp.StackTrace);
            }
        }


        [HttpDelete]
        [Route("merchant/{id:long}")]
        public async Task<Response<bool>> DeleteMerchant([FromRoute]long id)
        {
            try
            {
                var res =await se.RemoveAsync(id,new LocationModel() { LocationName=DefaultText.NoText});
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message,exp.StackTrace);
            }
        }


        //locationEndpoints
        [HttpDelete]
        [Route("location/{id:long}")]
        public async Task<Response<bool>> DeleteLocation([FromRoute]long id)
        {
            try
            {
                var res = await se.RemoveAsync(id,new LocationModel() { LocationName=DefaultText.NoText});
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpGet]
        [Route(nameof(GetLocation))]
        public async Task<Response<IEnumerable<LocationModel>>> GetLocation()
        {
            try
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
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<IEnumerable<LocationModel>>.Error(exp.Message, exp.Message);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<LocationModel>>> AllActiveLocation()
        {
            try
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
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<IEnumerable<LocationModel>>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpGet]
        [Route("Location/{id:long}")]
        public async Task<Response<LocationModel>> GetLocation([FromRoute] long id)
        {
            try
            {
                var cacheKey = $"getAllLocationById{id}";
                if (cashMemoryCache.TryGetValue(cacheKey, out LocationModel? mod))
                {
                    if (mod != null) return Response<LocationModel>.Ok(mod);
                }

                var res = await se.GetByIdAsync(id, new LocationModel() { LocationName = DefaultText.NoText });
                if (res is not null)
                {
                    return Response<LocationModel>.Error(ErrorKeys.BadRequest);
                }
                cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
                return Response<LocationModel>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<LocationModel>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpPost]
        [Route(nameof(InsertLocation))]
        public async Task<Response<long>> InsertLocation([FromBody] LocationModel value)
        {
            try
            {
                if (!ModelState.IsValid || value is null) return Response<long>.Error(ErrorKeys.BadRequest);
                var res = await se.AddAsync(value);
                return Response<long>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<long>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpPut]
        [Route("Location/{id:long}")]
        public async Task<Response<bool>> Update([FromRoute]long id, [FromBody] LocationModel value)
        {
            try
            {
                if (!ModelState.IsValid || value is null) return Response<bool>.Error(ErrorKeys.BadRequest);
                var res = await se.UpdateAsync(id, value);
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message,exp.StackTrace);
            }
        }


        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<Response<bool>> DeleteLocationSoft([FromRoute]long id)
        {

            try
            {
                var res = await se.SoftDeleteAsync(id, new LocationModel() { LocationName = DefaultText.NoText });
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return Response<bool>.Error(exp.Message,exp.StackTrace);
            }
        }
    }
}
