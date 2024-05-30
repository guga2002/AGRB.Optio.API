using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantRelatedService ser;
        private readonly ILogger<MerchantController> log;
        private readonly IMemoryCache cache;

        public MerchantController(IMerchantRelatedService se, ILogger<MerchantController> log,IMemoryCache _cache)
        {
            this.ser = se;
            this.log = log;
            this.cache= _cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string cacheKey = "GetAllMerchantsKey";

                if (cache.TryGetValue(cacheKey, out IEnumerable<MerchantModel>? cachedData))
                {
                    return Ok(cachedData);
                }
                else
                {
                    var res = await ser.GetAllAsync(new MerchantModel() { Name = "Undefined", });
                    if (!res.Any())
                    {
                        return NotFound();
                    }
                    cache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
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
        [Route("Merchant/{Merchantid:long}/Location/{Locationid:long}")]
        public async Task<IActionResult> AssignLocationtoMerchant([FromRoute]long Merchantid,[FromRoute] long Locationid)
        {
            try
            {
                var res = await ser.AssignLocationtoMerchant(Merchantid, Locationid);
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
        [Route("[action]")]
        public async Task<IActionResult> AllActive()
        {
            try
            {
                string cacheKey = "GetAllActiveMerchantsKey";

                if (cache.TryGetValue(cacheKey, out IEnumerable<MerchantModel>? cachedData))
                {
                    return Ok(cachedData);
                }
                else
                {
                    var res = await ser.GetAllActiveAsync(new MerchantModel() { Name = "Undefined" });
                    if (!res.Any())
                    {
                        return NotFound();
                    }
                    cache.Set(cacheKey, res, TimeSpan.FromMinutes(20));
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
        [Route("{id:long}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            try
            {

                string cashedkey = $"GetValueById{id}";

                if (cache.TryGetValue(cashedkey, out MerchantModel? Value))
                {
                    return Ok(Value);
                }
                else
                {
                    var res = await ser.GetByIdAsync(id, new MerchantModel() { Name = "Undefined" });
                    if (res is null)
                    {
                        return NotFound();
                    }
                    cache.Set(cashedkey, res, TimeSpan.FromMinutes(20));
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
        public async Task<IActionResult> Insert([FromBody] MerchantModel value)
        {
            try
            {
                if(ModelState.IsValid&&value is not null)
                {
                  var res= await ser.AddAsync(value);
                   if(res != -1)
                    {
                        return Ok(res);
                    }
                   return StatusCode(405,"SOmethings unusual");
                }
                return BadRequest(value);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
               return BadRequest(exp.Message);
            }
        }


        [HttpPut]
        [Route("{id:long}")]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] MerchantModel value)
        {
            try
            {
                if (ModelState.IsValid && value is not null)
                {
                    var res = await ser.UpdateAsync(id,value);
                    if (res)
                    {
                        return Ok(res);
                    }
                    return StatusCode(405, "SOmethings unusual");
                }
                return BadRequest(value);
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
                var res = await ser.SoftDeleteAsync(id, new MerchantModel() { Name = "Undefined" });
                if (res)
                {
                    return Ok(res);
                }
                return StatusCode(405, "Somethings unusual");
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpDelete]
        [Route("merchant/{id:long}")]
        public async Task<IActionResult> Deletemerchant([FromRoute]long id)
        {
            try
            {
                var res =await ser.RemoveAsync(id,new locationModel() { LocationName="undefined"});
                return res == true ? Ok("success") : BadRequest("somethings went wrong");
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
                var res = await ser.RemoveAsync(id,new locationModel() { LocationName="Undefined"});
                return res == true ? Ok("succesfully deleted") : BadRequest("soemthings went wrong");
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetLocation()
        {
            try
            {
                string cashedkey = "GetLocation";
                if (cache.TryGetValue(cashedkey, out IEnumerable<locationModel>? mod))
                {
                    return Ok(mod);
                }
                else
                {
                    var res = await ser.GetAllAsync(new locationModel() { LocationName = "Undefined" });
                    if (!res.Any())
                    {
                        return NotFound();
                    }
                    cache.Set(cashedkey, res, TimeSpan.FromMinutes(15));
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
        [Route("[action]")]
        public async Task<IActionResult> AllActiveLocation()
        {
            try
            {
                var cashed = "GetAllActiveLocation";

                if (cache.TryGetValue(cashed, out IEnumerable<locationModel>? loc))
                {
                    return Ok(loc);
                }
                else
                {
                    var res = await ser.GetAllActiveAsync(new locationModel() { LocationName = "Undefined" });
                    if (!res.Any())
                    {
                        return NotFound();
                    }
                    cache.Set(cashed, res, TimeSpan.FromMinutes(15));
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
        [Route("Location/{id:long}")]
        public async Task<IActionResult> GetLocation([FromRoute] long id)
        {
            try
            {
                var cashedkey = $"getalllocationbyid{id}";
                if (cache.TryGetValue(cashedkey, out locationModel? mod))
                {
                    return Ok(mod);
                }
                else
                {
                    var res = await ser.GetByIdAsync(id, new locationModel() { LocationName = "Undefined" });
                    if (res is not null)
                    {
                        return NotFound();
                    }
                    cache.Set(cashedkey, res, TimeSpan.FromMinutes(15));
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
        [Route("Location")]
        public async Task<IActionResult> Insert([FromBody] locationModel value)
        {
            try
            {
                if (ModelState.IsValid && value is not null)
                {
                    var res = await ser.AddAsync(value);
                    if (res != -1)
                    {
                        return Ok(res);
                    }
                    return StatusCode(405, "Somethings unusual");
                }
                return BadRequest(value);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpPut]
        [Route("Location/{id:long}")]
        public async Task<IActionResult> Update([FromRoute]long id, [FromBody] locationModel value)
        {
            try
            {
                if (ModelState.IsValid && value is not null)
                {
                    var res = await ser.UpdateAsync(id, value);
                    if (res)
                    {
                        return Ok(res);
                    }
                    return StatusCode(405, "Somethings unusual");
                }
                return BadRequest(value);
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
                var res = await ser.SoftDeleteAsync(id, new locationModel() { LocationName = "Undefined" });
                if (res)
                {
                    return Ok(res);
                }
                return StatusCode(405, "Somethings unusual");
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message, exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }
    }
}
