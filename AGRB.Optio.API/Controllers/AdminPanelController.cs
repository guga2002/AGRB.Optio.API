using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AGRB.Optio.Domain.Custom_Exceptions;

namespace AGRB.Optio.API.Controllers
{
    /// <summary>
    /// Controller for admin panel operations.
    /// </summary>
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IAdminPanelService panel;

        /// Initializes a new instance of the <see cref="AdminPanelController"/> class.
        /// <param name="panel">The admin panel service.</param>
        public AdminPanelController(IAdminPanelService panel)
        {
                this.panel = panel;
        }
        /// <summary>
        /// Gets all roles. V2.0
        /// </summary>
        /// <returns>A response containing a list of roles.</returns>
        /// <remarks>
        /// This endpoint retrieves all roles available in the system.**avalible for only admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<RoleModel>>> Roles()
        {
            var res = await panel.GetAllRoles();
            return Response<IEnumerable<RoleModel>>.Ok(res);
        }

        /// <summary>
        /// Gets all Users. V2.0
        /// </summary>
        /// <returns>A response containing a list of Users.</returns>
        /// <remarks>
        /// This endpoint retrieves all Users available in the system.**Require Role Admin**
        /// </remarks>
        [HttpGet]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IEnumerable<UserModel>>> Users()
        {
            var res = await panel.GetAllUser();
            return Response<IEnumerable<UserModel>>.Ok(res);
        }

        /// <summary>
        /// Delete Role from DB V2.0
        /// </summary>
        /// <returns>A response is IdentityResult.</returns>
        /// <remarks>
        /// Avalible for only **Admin**
        /// </remarks>
        [HttpDelete]
        [Route("Role/{role:alpha}/[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IdentityResult>> Delete([FromRoute] string role)
        {
            var res = await panel.DeleteRole(role);
            return Response<IdentityResult>.Ok(res);
        }

        /// <summary>
        ///Add new role to Db V2.0
        /// </summary>
        /// <returns>A response containing IdentityResult</returns>
        /// <remarks>
        /// This endpoint avalible for only **Admin**
        /// </remarks>
        [HttpPost]
        [Route("Role/{role:alpha}/[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IdentityResult>> Add([FromRoute] string role)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(role);
            }
            var res = await panel.AddRolesAsync(role);
            return Response<IdentityResult>.Ok(res);
        }

        /// <summary>
        /// Delete User From DB V2.0
        /// </summary>
        /// <returns>A response containing identityResult</returns>
        /// <remarks>
        /// This endpint avalible for **Admin**
        /// </remarks>
        [HttpDelete]
        [Route("User/{id}/[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IdentityResult>> DeleteUser([FromRoute] string id)
        {
            var res = await panel.DeleteUser(id);
            return Response<IdentityResult>.Ok(res);
        }

        /// <summary>
        /// Assign Role to specify user V2.0
        /// </summary>
        /// <returns>A response containing identityResult</returns>
        /// <remarks>
        /// This endpint avalible for **Admin**
        /// </remarks>
        [HttpPost]
        [Route("User/{userid}[action]/{role:alpha}")]
        [MapToApiVersion("2.0")]
        public async Task<Response<IdentityResult>> Role([FromRoute] string userid, [FromRoute] string role)
        {
            var res = await panel.AssignRoleToUserAsync(userid, role);
            return Response<IdentityResult>.Ok(res);
        }
    }
}
