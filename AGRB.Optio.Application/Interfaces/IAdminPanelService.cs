using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Application.Models.ResponseModels;
using AGRB.Optio.Infrastructure.Identity.HelperModels;
using Microsoft.AspNetCore.Identity;

namespace AGRB.Optio.Application.Interfaces
{
    public interface IAdminPanelService
    {
        Task<IdentityResult> DeleteRole(string role);
        Task<UserModel> Info(string username);
        Task<bool> ForgetPassword(string email, string newPassword);
        Task<AuthResult> RefreshToken(TokenRequest tok);
        Task<AuthResult> RegisterUserAsync(UserModel user, string password);
        Task<AuthResult> SignInAsync(SignInModel mod);
        Task<IdentityResult> AddRolesAsync(string roleName);
        Task<IdentityResult> AssignRoleToUserAsync(string userId, string role);
        Task<IdentityResult> ResetPasswordAsync(PasswordResetModel arg, string username);
        Task<bool> SignOutAsync(string username);
        Task<IdentityResult> DeleteUser(string id);
        Task<IEnumerable<RoleModel>> GetAllRoles();
        Task<IEnumerable<UserModel>> GetAllUser();
        Task<bool> IsEmailConfirmed(string email);
        Task<bool> ConfirmMail(string username, string mail);
        Task<bool> SendLinkToUser(string name, string link);
        Task<bool> IsUserExist(string email);
    }
}
