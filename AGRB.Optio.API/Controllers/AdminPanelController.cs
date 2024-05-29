using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Models.RequestModels;
using RGBA.Optio.Domain.Responses;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class AdminPanelController(IAdminPanelService panel, ILogger<AdminPanelController> co) : ControllerBase
    {

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<RoleModel>>> Roles()
        {
            try
            {
               var res= await panel.GetAllRoles();
                return  Response<IEnumerable<RoleModel>>.Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return Response < IEnumerable < RoleModel >>.Error(exp.Message,exp.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<UserModel>>> Users()
        {
            try
            {
                var res = await panel.GetAllUser();
                return Response<IEnumerable<UserModel>>.Ok (res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return Response<IEnumerable<UserModel>>.Error(exp.Message,exp.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Role/{role:alpha}/[action]")]
        public async Task<Response<IdentityResult>> Delete([FromRoute]string role)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    throw new OptioGeneralException(role);
                }
                var res=await panel.DeleteRole(role);
                return Response<IdentityResult>.Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
              return  Response<IdentityResult>.Error(exp.Message, exp.StackTrace);
            }
        }

        [HttpPost]
        [Route("Role/{role:alpha}/[action]")]
        public async Task<Response<IdentityResult>> Add([FromRoute]string role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(role);
                }
                var res = await panel.AddRolesAsync(role);
                return Response<IdentityResult>.Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return Response<IdentityResult>.Error(exp.Message,exp.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("User/{id}/[action]")]
        public async Task<Response<IdentityResult>> DeleteUser([FromRoute]string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(id);
                }
                var res = await panel.DeleteUser(id);
                return Response<IdentityResult>.Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return Response<IdentityResult>.Error(exp.Message, exp.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("User/{userid}[action]/{role:alpha}")]
        public async Task<Response<IdentityResult>> Role([FromRoute] string userid,[FromRoute]string role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(role);
                }
                var res = await panel.AssignRoleToUserAsync(userid,role);
                return Response<IdentityResult>.Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return Response<IdentityResult>.Error(exp.Message,exp.StackTrace, ErrorKeys.InternalServerError);
            }
        }
    }
}
