using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

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
        public async Task<IActionResult> GetAllMerchants()
        {
            try
            {
                const string cacheKey = "GetAllMerchantsKey";

                if (cashMemoryCache.TryGetValue(cacheKey, out IEnumerable<MerchantModel>? cachedData))
                {
                    return Ok(cachedData);
                }
                else
                {
                    var res = await se.GetAllAsync(new MerchantModel() { Name = "Undefined", });
                    if (!res.Any())
                    {
                        return NotFound();
                    }
                    cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                    return Ok(res);
                }
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }


        [HttpPost]
        [Route("Merchant/{merchantId:long}/Location/{locationId:long}")]
        public async Task<IActionResult> AssignLocationToMerchant([FromRoute]long merchantId,[FromRoute] long locationId)
        {
            try
            {
                var res = await se.AssignLocationToMerchant(merchantId, locationId);
                if(res)
                {
                    return Ok(res);
                }
                return StatusCode(404);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }


        [HttpGet]
        [Route(nameof(AllActiveMerchant))]
        public async Task<IActionResult> AllActiveMerchant()
        {
            try
            {
                const string cacheKey = "GetAllActiveMerchantsKey";

                if (cashMemoryCache.TryGetValue(cacheKey, out IEnumerable<MerchantModel>? cachedData))
                {
                    return Ok(cachedData);
                }
                else
                {
                    var res = await se.GetAllActiveAsync(new MerchantModel() { Name = "Undefined" });
                    if (!res.Any())
                    {
                        return NotFound();
                    }
                    cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                    return Ok(res);
                }
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }


        [HttpGet]
        [Route("Merchant/{id:long}")]
        public async Task<IActionResult> GetMerchant([FromRoute] long id)
        {
            try
            {

                var cacheKey = $"GetValueById{id}";

                if (cashMemoryCache.TryGetValue(cacheKey, out MerchantModel? value))
                {
                    return Ok(value);
                }
                else
                {
                    var res = await se.GetByIdAsync(id, new MerchantModel() { Name = "Undefined" });
                    if (res is null)
                    {
                        return NotFound();
                    }
                    cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
                    return Ok(res);
                }
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }


        [HttpPost]
        [Route(nameof(InsertMerchant))]
        public async Task<IActionResult> InsertMerchant([FromBody] MerchantModel value)
        {
            try
            {
                if (!ModelState.IsValid || value is null) return BadRequest(value);
                var res= await se.AddAsync(value);
                return res != -1 ? Ok(res) : StatusCode(405,ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
               return BadRequest(exp.Message);
            }
        }


        [HttpPut]
        [Route("Merchant/{id:long}")]
        public async Task<IActionResult> UpdateMerchant([FromRoute] long id, [FromBody] MerchantModel value)
        {
            try
            {
                if (!ModelState.IsValid || value is null) return BadRequest(value);
                var res = await se.UpdateAsync(id,value);
                return res ? Ok(res) : StatusCode(405, ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult>  Delete([FromRoute] long id)
        {

            try
            {
                var res = await se.SoftDeleteAsync(id, new MerchantModel() { Name = "Undefined" });
                return res ? Ok(res) : StatusCode(405, ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpDelete]
        [Route("merchant/{id:long}")]
        public async Task<IActionResult> DeleteMerchant([FromRoute]long id)
        {
            try
            {
                var res =await se.RemoveAsync(id,new LocationModel() { LocationName="undefined"});
                return res ? Ok(SuccessKeys.Success) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        //locationEndpoints
        [HttpDelete]
        [Route("location/{id:long}")]
        public async Task<IActionResult> DeleteLocation([FromRoute]long id)
        {
            try
            {
                var res = await se.RemoveAsync(id,new LocationModel() { LocationName="Undefined"});
                return res ? Ok(SuccessKeys.Success) : BadRequest(ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpGet]
        [Route(nameof(GetLocation))]
        public async Task<IActionResult> GetLocation()
        {
            try
            {
                const string cacheKey = "GetLocation";
                if (cashMemoryCache.TryGetValue(cacheKey, out IEnumerable<LocationModel>? mod))
                {
                    return Ok(mod);
                }

                var res = await se.GetAllAsync(new LocationModel() { LocationName = "Undefined" });
                if (!res.Any())
                {
                    return NotFound();
                }
                cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> AllActiveLocation()
        {
            try
            {
                const string cashed = "GetAllActiveLocation";

                if (cashMemoryCache.TryGetValue(cashed, out IEnumerable<LocationModel>? loc))
                {
                    return Ok(loc);
                }

                var res = await se.GetAllActiveAsync(new LocationModel() { LocationName = "Undefined" });
                if (!res.Any())
                {
                    return NotFound();
                }
                cashMemoryCache.Set(cashed, res, TimeSpan.FromMinutes(15));
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }


        [HttpGet]
        [Route("Location/{id:long}")]
        public async Task<IActionResult> GetLocation([FromRoute] long id)
        {
            try
            {
                var cacheKey = $"getAllLocationById{id}";
                if (cashMemoryCache.TryGetValue(cacheKey, out LocationModel? mod))
                {
                    return Ok(mod);
                }

                var res = await se.GetByIdAsync(id, new LocationModel() { LocationName = "Undefined" });
                if (res is not null)
                {
                    return NotFound();
                }
                cashMemoryCache.Set(cacheKey, res, TimeSpan.FromMinutes(15));
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }


        [HttpPost]
        [Route(nameof(InsertLocation))]
        public async Task<IActionResult> InsertLocation([FromBody] LocationModel value)
        {
            try
            {
                if (!ModelState.IsValid || value is null) return BadRequest(value);
                var res = await se.AddAsync(value);
                return res != -1 ? Ok(res) : StatusCode(405, ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpPut]
        [Route("Location/{id:long}")]
        public async Task<IActionResult> Update([FromRoute]long id, [FromBody] LocationModel value)
        {
            try
            {
                if (!ModelState.IsValid || value is null) return BadRequest(value);
                var res = await se.UpdateAsync(id, value);
                return res ? Ok(res) : StatusCode(405, ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpPost]
        [Route("[action]/{id:long}")]
        public async Task<IActionResult> DeleteLocationSoft([FromRoute]long id)
        {

            try
            {
                var res = await se.SoftDeleteAsync(id, new LocationModel() { LocationName = "Undefined" });
                return res ? Ok(res) : StatusCode(405, ErrorKeys.BadRequest);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }
    }
}
