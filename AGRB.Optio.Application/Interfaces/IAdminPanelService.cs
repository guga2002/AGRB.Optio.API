using Microsoft.AspNetCore.Identity;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Models.RequestModels;

namespace RGBA.Optio.Domain.Interfaces
{
    public interface IAdminPanelService
    {
        Task<IdentityResult> DeleteRole(string role);
        Task<UserModel> Info(string username);  
        Task<bool> ForgetPassword(string email, string newPassword); 
        Task<bool> RefreshToken(string username,string token);
        Task<IdentityResult> RegisterUserAsync(UserModel user, string password);
        Task<(SignInResult, string)> SignInAsync(SignInModel mod);
        Task<IdentityResult> AddRolesAsync(string roleName);
        Task<IdentityResult> AssignRoleToUserAsync(string userId, string role);
        Task<IdentityResult> ResetPasswordAsync(PasswordResetModel arg,string username);
        Task<bool> SignOutAsync(string username);
        Task<IdentityResult> DeleteUser(string id);
        Task<IEnumerable<RoleModel>> GetAllRoles();
        Task<IEnumerable<UserModel>> GetAllUser();
        Task<bool> IsEmailConfirmed(string email);
        Task<bool> ConfirmMail(string username,string mail);
        Task<bool> SendLinkToUser(string name, string link);
        Task<bool> IsUserExist(string email);
    }
}
