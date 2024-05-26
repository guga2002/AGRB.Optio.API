using Microsoft.AspNetCore.Identity;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Models.RequestModels;

namespace RGBA.Optio.Domain.Interfaces
{
    public interface IAdminPanelService
    {
        Task<IdentityResult> DeleteRole(string role);
        Task<UserModel> Info(string Username);  
        Task<bool> ForgetPassword(string email, string newPassword); 
        Task<bool> RefreshToken(string Username,string token);
        Task<IdentityResult> RegisterUserAsync(UserModel User, string Password);
        Task<(SignInResult, string)> SignInAsync(SignInModel mod);
        Task<IdentityResult> AddRolesAsync(string RoleName);
        Task<IdentityResult> AssignRoleToUserAsync(string UserId, string Role);
        Task<IdentityResult> ResetPasswordAsync(PasswordResetModel arg,string username);
        Task<bool> SignOutAsync(string Username);
        Task<IdentityResult> DeleteUser(string id);
        Task<IEnumerable<RoleModel>> GetAllRoles();
        Task<IEnumerable<UserModel>> GetAllUser();
        Task<bool> isEmailConfirmed(string email);
        Task<bool> ConfirmMail(string Username,string mail);
        Task<bool> sendlinktouser(string name, string link);
        Task<bool> IsUserExist(string email);
    }
}
