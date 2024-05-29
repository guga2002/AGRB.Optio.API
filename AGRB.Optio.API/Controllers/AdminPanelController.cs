using AGRB.Optio.API.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class AdminPanelController(IAdminPanelService panel, ILogger<AdminPanelController> co) : ControllerBase
    {

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Roles()
        {
            try
            {
               var res= await panel.GetAllRoles();
                return Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Users()
        {
            try
            {
                var res = await panel.GetAllUser();
                return Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Role/{role:alpha}/[action]")]
        public async Task<IActionResult> Delete([FromRoute]string role)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    throw new OptioGeneralException(role);
                }
                var res=await panel.DeleteRole(role);
                return Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Role/{role:alpha}/[action]")]
        public async Task<IActionResult> Add([FromRoute]string role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(role);
                }
                var res = await panel.AddRolesAsync(role);
                return Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("User/{id}/[action]")]
        public async Task<IActionResult> DeleteUser([FromRoute]string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(id);
                }
                var res = await panel.DeleteUser(id);
                return Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("User/{userid}[action]/{role:alpha}")]
        public async Task<IActionResult> Role([FromRoute] string userid,[FromRoute]string role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(role);
                }
                var res = await panel.AssignRoleToUserAsync(userid,role);
                return Ok(res);
            }
            catch (Exception exp)
            {
                co.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }
    }
}
