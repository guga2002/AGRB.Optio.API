using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class AdminPanellController : ControllerBase
    {
        private readonly IAdminPanelService panel;
        private readonly ILogger<AdminPanellController> log;
        public AdminPanellController(IAdminPanelService panel,ILogger<AdminPanellController> co)
        {
            this.panel = panel;
            this.log = co;
        }
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
                log.LogCritical(exp.Message);
                return StatusCode(503, "Internal Server Errror");
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
                log.LogCritical(exp.Message);
                return StatusCode(503, "Internal Server Errror");
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
                log.LogCritical(exp.Message);
                return StatusCode(503, "Internal Server Errror");
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
                log.LogCritical(exp.Message);
                return StatusCode(503, "Internal Server Errror");
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
                log.LogCritical(exp.Message);
                return StatusCode(503, "Internal Server Errror");
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
                log.LogCritical(exp.Message);
                return StatusCode(503, "Internal Server Errror");
            }
        }
    }
}
