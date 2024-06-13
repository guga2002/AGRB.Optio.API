using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Responses;

namespace AGRB.Optio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeadBackController : ControllerBase
    {
        private readonly IFeadbackService ser;

        public FeadBackController(IFeadbackService ser)
        {
            this.ser = ser;
        }

        [HttpPost]
        public async Task<Response<long>> AddAsync([FromBody]FeadbackModel entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   await  ser.AddAsync(entity);
                }
                return Response<long>.Error(nameof(entity));
            }
            catch (Exception exp)
            {
                return Response<long>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpGet]
        [Route("AllActive")]
        public async Task<Response<IEnumerable<FeadbackModel>>> GetAllActiveAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                 var res=await  ser.GetAllActiveAsync(new FeadbackModel() { FeadBack="Default",UserId="default"});

                    return Response<IEnumerable<FeadbackModel>>.Ok(res);
                }
                return Response<IEnumerable<FeadbackModel>>.Error(ModelState.ToString());
            }
            catch (Exception exp)
            {
                return Response<IEnumerable<FeadbackModel>>.Error(exp.Message, exp.StackTrace);
            }
        }
        [HttpGet]
        public async Task<Response<IEnumerable<FeadbackModel>>> GetAllAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await ser.GetAllAsync(new FeadbackModel() { FeadBack = "Default", UserId = "default" });

                    return Response<IEnumerable<FeadbackModel>>.Ok(res);
                }
                return Response<IEnumerable<FeadbackModel>>.Error(ModelState.ToString());
            }
            catch (Exception exp)
            {
                return Response<IEnumerable<FeadbackModel>>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<Response<FeadbackModel>> GetByIdAsync(long id)
        {
            try
            {
                var res = await ser.GetByIdAsync(id, new FeadbackModel() { FeadBack = "Default", UserId = "default" });
                if (res is not null)
                {
                    return Response<FeadbackModel>.Ok(res);
                }
                return Response<FeadbackModel>.Error(id.ToString());
            }
            catch (Exception exp)
            {
                return Response<FeadbackModel>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpDelete]
        [Route("{id:long}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<Response<bool>> RemoveAsync(long id)
        {
            try
            {
                var res = await ser.RemoveAsync(id, new FeadbackModel() { FeadBack = "Default", UserId = "default" });
                if (res == false)
                {
                    return Response<bool>.Error(id.ToString());
                }
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                return Response<bool>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpPost]
        [Route("{id:long}")]
        [Authorize(Roles ="Admin,Manager")]
        public async Task<Response<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var res = await ser.SoftDeleteAsync(id, new FeadbackModel() { FeadBack = "Default", UserId = "default" });
                if (res == false)
                {
                    return Response<bool>.Error(id.ToString());
                }
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                return Response<bool>.Error(exp.Message, exp.StackTrace);
            }
        }
    }
}
