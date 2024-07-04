using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGRB.Optio.API.Controllers
{
    /// <summary>
    /// Controller for Feadback Related Actions
    /// </summary>
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class FeadBackController : ControllerBase
    {
        private readonly IFeadbackService ser;

        /// Initializes a new instance of the <see cref="FeadBackController"/> class.
        /// <param name="ser">The feadback service.</param>
        public FeadBackController(IFeadbackService ser)
        {
            this.ser = ser;
        }
        /// <summary>
        ///Add New Feadbacks  to db V2.0
        /// </summary>
        /// <returns>A response containing long </returns>
        /// <remarks>
        ///  avalible for **Authorized members**
        /// </remarks>
        [HttpPost]
        [MapToApiVersion("2.0")]
        public async Task<Response<long>> AddAsync([FromBody] FeadbackModel entity)
        {
            var res = await ser.AddAsync(entity);
            return Response<long>.Ok(res);
        }

        /// <summary>
        /// Retrive All Active Feadbacks V1.0
        /// </summary> 
        /// <returns>A response containing aList of Feadbacks </returns>
        /// <remarks>
        ///  avalible for **Admin ,manager, Operator**
        /// </remarks>
        [HttpGet]
        [Route("AllActive")]
        [MapToApiVersion("1.0")]
        public async Task<Response<IEnumerable<FeadbackModel>>> GetAllActiveAsync()
        {
            var res = await ser.GetAllActiveAsync(new FeadbackModel() { FeadBack = "Default", UserId = "default" });
            return Response<IEnumerable<FeadbackModel>>.Ok(res);
        }

        /// <summary>
        /// Retrive All Feadbacks V2.0
        /// </summary>
        /// <returns>A response containing aList of Feadbacks </returns>
        /// <remarks>
        ///  avalible for **Admin ,manager, Operator**
        /// </remarks>
        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<FeadbackModel>>> GetAllAsync()
        {

            var res = await ser.GetAllAsync(new FeadbackModel() { FeadBack = "Default", UserId = "default" });
            return Response<IEnumerable<FeadbackModel>>.Ok(res);
        }

        /// <summary>
        /// Retrive Feadback by id V2.0
        /// </summary>
        /// <returns>A response containing a feadback </returns>
        /// <remarks>
        ///  avalible for **Admin ,manager, Operator**
        /// </remarks>
        [HttpGet]
        [Route("{id:long}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<FeadbackModel>> GetByIdAsync(long id)
        {
            var res = await ser.GetByIdAsync(id, new FeadbackModel() { FeadBack = "Default", UserId = "default" });
            if (res is not null)
            {
                return Response<FeadbackModel>.Ok(res);
            }
            return Response<FeadbackModel>.Error(id.ToString());
        }

        /// <summary>
        ///Remove  Feadback from Database V2.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **Admin ,manager, Operator**
        /// </remarks>
        [HttpDelete]
        [Route("{id:long}")]
        [Authorize(Roles = "Admin,Manager,Operator")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> RemoveAsync(long id)
        {
            var res = await ser.RemoveAsync(id, new FeadbackModel() { FeadBack = "Default", UserId = "default" });
            if (res == false)
            {
                return Response<bool>.Error(id.ToString());
            }
            return Response<bool>.Ok(res);
        }

        /// <summary>
        ///Soft Delete  Feadback from Database V1.0
        /// </summary>
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **Admin ,manager, Operator**
        /// </remarks>
        [HttpPost]
        [Route("{id:long}")]
        [Authorize(Roles = "Admin,Manager,Operator")]
        [MapToApiVersion("1.0")]
        public async Task<Response<bool>> SoftDeleteAsync(long id)
        {
            var res = await ser.SoftDeleteAsync(id, new FeadbackModel() { FeadBack = "Default", UserId = "default" });
            if (res == false)
            {
                return Response<bool>.Error(id.ToString());
            }
            return Response<bool>.Ok(res);
        }
    }
}
