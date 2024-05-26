using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Models.RequestModels;
using RGBA.Optio.Domain.Services.Outer_Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RGBA.Optio.Domain.Services
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly RoleManager<IdentityRole> role;
        private readonly SignInManager<User> signin;
        private readonly UserManager<User> userManager;
        private readonly IMapper map;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SmtpService smtp;
        public AdminPanelService(IHttpContextAccessor _httpContextAccessor, IMapper map, SignInManager<User> signin, UserManager<User> userManager,RoleManager<IdentityRole> rol, SmtpService smtp)
        {
            this.signin = signin;
            this.userManager = userManager;
            role = rol;
            this.map = map;
            this._httpContextAccessor = _httpContextAccessor;
            this.smtp = smtp;
        }

        #region AddRolesAsync

        public async Task<IdentityResult> AddRolesAsync(string RoleName)
        {
            try
            {
                if(string.IsNullOrEmpty(RoleName)||RoleName.Length<=3)
                {
                    throw new OptioGeneralException(RoleName);
                }
                var res = await role.CreateAsync(new IdentityRole(RoleName));
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region isEmailConfirmed
        public async Task<bool> isEmailConfirmed(string email)
        {
            var res = await userManager.FindByNameAsync(email);
            if(res is not null)
            {
                return await userManager.IsEmailConfirmedAsync(res);
            }
            return false;
        }
        #endregion

        #region IsUserExist

        public async Task<bool> IsUserExist(string email)
        {
            var res = await userManager.FindByEmailAsync(email);
          
            if(res is not null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region ConfirmMail
        public async Task<bool> ConfirmMail(string Username, string mail)
        {
            try
            {
                var user = await userManager.FindByNameAsync(Username);
                if (user is not null&&user.UserName is not null)
                {
                    if (await isEmailConfirmed(user.UserName))
                    {
                        return false;
                    }
                    await userManager.ConfirmEmailAsync(user, mail);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region sendlinktouser
        public async Task<bool> sendlinktouser(string name, string link)
        {
            var res=await userManager.FindByNameAsync(name);
            if (res == null || res.Email is null) return false;
            var body = $@"
                  <div align='center' style='font-family: Arial, sans-serif;'>
                  <p style='font-size: 16px;'>ძვირასო {res.Name},</p>
                  <p style='font-size: 16px;'>გთხოვთ დაადასტუროთ თქვენი მეილი, ამ ლინკზე გადასვლით:</p>
                 <p style='font-size: 16px;'>
                 <a href='{link}' style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 5px;'>დაადასტურე მეილი</a>
                 </p>
                 <p style='font-size: 16px;'>ლინკი ვალიდურია 24 საათის განავლობაში</p>
                 <p style='font-size: 16px;'>ჩვენი ჯგუფი გიხდის მადლობას..</p>
                  <h2 style='font-size: 16px;color:red;'>თუ თქვენ  არ გამოგიგზავნიათ მოთხოვნა, გთხოვთ დაგვიკავშირდეთ!</h2>
                </div>";

            smtp.SendMessage(res.Email, "დაადასტურე მეილი"+' '+DateTime.Now.Hour+':'+DateTime.Now.Minute, body);
            return true;
        }
        #endregion

        #region AssignRoleToUserAsync

        public async Task<IdentityResult> AssignRoleToUserAsync(string UserId, string Role)
        {
            try
            {
                if (await role.RoleExistsAsync(Role))
                {
                    var res = userManager.Users.Where(io => io.Id == UserId).FirstOrDefault();
                    if (res is null)
                    {
                        return new IdentityResult();
                    }
                    var rek = await userManager.AddToRoleAsync(res, Role);
                    return rek;
                }
                return new IdentityResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DeleteRole
        public async Task<IdentityResult> DeleteRole(string rol)
        {
            try
            {
                if(await role.RoleExistsAsync(rol))
                {
                    var res = await role.DeleteAsync(new IdentityRole(rol));
                    return res;
                }
                return new IdentityResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DeleteUser
        public async Task<IdentityResult> DeleteUser(string id)
        {
            var res = userManager.Users.FirstOrDefault(io => io.Id == id);
            if(res is not null)
            {
                var rek=await userManager.DeleteAsync(res);
                return rek;
            }
            return new IdentityResult();
        }
        #endregion

        #region ForgetPassword
        public async Task<bool> ForgetPassword(string Email,string NewPassword)
        {
            var user=await userManager.FindByEmailAsync(Email);
          
            if ((user is not null))
            {
                var rej =await  userManager.CheckPasswordAsync(user, NewPassword);
                if(rej)
                {
                    return false;
                }
                var res = await userManager.GeneratePasswordResetTokenAsync(user);
                if (res is not null)
                {
                   var rek=await userManager.ResetPasswordAsync(user,res, NewPassword);
                   if(rek.Succeeded)
                    {
                        return true;
                    }
                }
                return false;
            }
            throw new ArgumentException("Such User no exist");
        }
        #endregion

        #region Info

        public async Task<UserModel> Info(string Username)
        {
            try
            {
                var res = await userManager.Users.FirstOrDefaultAsync(io => io.UserName == Username);
                if (res is not null)
                {
                    var mapped = map.Map<UserModel>(res);
                    return mapped;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region RefreshToken

        public async Task<bool> RefreshToken(string Username, string token)
        {
            var user = await userManager.FindByNameAsync(Username);
            if (user == null)
            {
                return false;
            }

            var isValidToken = await signin.UserManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider, "RefreshToken", token);
            if (!isValidToken)
            {
                return false;
            }

            var result = await signin.UpdateExternalAuthenticationTokensAsync(new ExternalLoginInfo(new System.Security.Claims.ClaimsPrincipal(),"JWT Bearer", "65E255FF-F399-42D4-9C7F-D5D08B0EC285","Auth")
            {
                ProviderKey = user.Id,
                AuthenticationTokens = new AuthenticationToken[]
                {
                new AuthenticationToken { Name = "access_token", Value = token },
                }
            });

            return result.Succeeded;
        }
        #endregion

        #region RegisterUserAsync
        public async Task<IdentityResult> RegisterUserAsync(UserModel User, string Password)
        {
            try
            {

                if (await userManager.FindByEmailAsync(User.Email) is null)
                {
                    var maped = map.Map<User>(User);
                    var res = await userManager.CreateAsync(maped, Password);
                    if (res.Succeeded)
                    {
                        var body = @"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Welcome to RGBASOLUTION!</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f8f9fa;
        }
        .container {
            width: 80%;
            margin: auto;
            margin-left:30%;
            padding: 20px;
        }
        .header {
            text-align: center;
            color: #007bff;
            font-size: 24px;
            margin-bottom: 20px;
        }
        .content {
            font-size: 16px;
            color: #333;
            margin-bottom: 15px;
        }
        .list-item {
            font-size: 16px;
            color: #333;
            margin-left: 20px;
        }
        .security-notice {
            background-color: #f8f9fa;
            padding: 10px;
            border-radius: 5px;
            margin-top: 20px;
            text-align: center;
            color: #555;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            Welcome to RGBASOLUTION!
        </div>
        <div class='content'>
            <p>Dear,</p>
            <p>Congratulations on creating your account.</p>
            <p>We are excited to have you on board!</p>
            <p>Here are some next steps to get started:</p>
            <ul>
                <li class='list-item'>Explore our features and services.</li>
                <li class='list-item'>Customize your profile settings.</li>
                <li class='list-item'>Contact our support team if you need any assistance.</li>
            </ul>
            <p>Thank you for choosing RGBASOLUTION!</p>
        </div>
        <div class='security-notice'>
            For security reasons, please do not share this email with anyone.
        </div>
    </div>
</body>
</html>
";

                        smtp.SendMessage(User.Email, "Welcome to RGBASOLUTION! Get Started Today", body);


                        return res;
                    }

                    throw new ArgumentException("somethings unucual");

                }
                throw new ArgumentException(" User already exist in db");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ResetPasswordAsync

        public async Task<IdentityResult> ResetPasswordAsync(PasswordResetModel arg, string username)
        {
            var res =await userManager.FindByNameAsync(username);
            if(res is not null)
            {
               var rek=await userManager.ChangePasswordAsync(res, arg.oldPassword, arg.newPassword);
                return rek;
            }
            return new IdentityResult();
        }
        #endregion

        #region SignInAsync

        public async Task<(Microsoft.AspNetCore.Identity.SignInResult, string)> SignInAsync(SignInModel mod)
        {
            if (string.IsNullOrEmpty(mod.Username) || string.IsNullOrEmpty(mod.Password))
            {
                throw new OptioGeneralException("Username or password is empty.");
            }

            var result = await signin.PasswordSignInAsync(mod.Username, mod.Password, mod.SetCookie, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                await SetPersistentCookieAsync(_httpContextAccessor.HttpContext.User);
                var token = GenerateJwtToken(mod.Username);
                await Console.Out.WriteLineAsync(token);
                var usr = await userManager.FindByNameAsync(mod.Username);
                if (usr != null)
                {
                    string recipientName = usr.Name+' '+usr.Surname;
                    string emailContent = $@"
                      <html>
                     <body style='font-family: Arial, sans-serif;'>
                     <p>Dear <span style='color: #3366cc;'>{recipientName}</span>,</p>
                     <p>We noticed a new sign-in to your RGBASOLUTION account. If this was you, there's no need for further action. 
                      However, if you didn't initiate this sign-in, please contact us immediately, and we will assist you in securing your account.</p>
                     <p>Thank you for your attention to this matter.</p>
                     <p style='color: #ff6600;'>Sincerely,<br/>Your RGBASOLUTION Team</p>
                     </body>
                     </html>";

                    smtp.SendMessage(usr.Email, $"Security Alert: New Sign-in to Your RGBASOLUTION Account {DateTime.Now.ToShortTimeString()}", emailContent);
                    await userManager.AddClaimAsync(usr, new Claim("Name", usr.Name));
                    await userManager.AddClaimAsync(usr, new Claim("Surname", usr.Surname));
                    await userManager.AddClaimAsync(usr, new Claim("PersonalNumber", usr.PersonalNumber));
                    await userManager.AddClaimAsync(usr, new Claim("BirthDay", usr.BirthDate.ToShortDateString()));
                    await userManager.AddLoginAsync(usr,new UserLoginInfo("JWT",GenerateJwtToken(usr.UserName),"Authorization"));
                }
                return (result, token);
            }
            else if (!result.Succeeded && !mod.SetCookie)
            {
                //await ClearPersistentCookieAsync();
            }

            return (null, null);
        }
        #endregion

        #region GenerateJwtToken

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
              new Claim(ClaimTypes.Name, username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KkQl/Fp7eupD0YdLsK+ynGpEZ6g/Y0N6/J4I2V57E8E="));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:44359/",
                audience: "https://localhost:44359/",
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region SetPersistentCookieAsync&&SetPersistentCookieAsync

        private async Task SetPersistentCookieAsync(ClaimsPrincipal principal)
        {
            await _httpContextAccessor.HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,principal, new AuthenticationProperties { IsPersistent = true });
        }

        private async Task SetPersistentCookieAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        }
        #endregion

        #region SignOutAsync
        public async Task<bool> SignOutAsync(string Username)
        {
            try
            {
                var user = await userManager.FindByNameAsync(Username);
                if (user is not null)
                {
                    await userManager.RemoveClaimsAsync(user, await userManager.GetClaimsAsync(user));
                    await signin.SignOutAsync();
                   var login=await  userManager.GetLoginsAsync(user);
                    if (login is not null)
                    {
                        var first = login.FirstOrDefault();
                        if (first is not null)
                        {
                            await userManager.RemoveLoginAsync(user, first.LoginProvider, first.ProviderKey);
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAllRoles

        public async Task<IEnumerable<RoleModel>> GetAllRoles()
        {
            var res=await role.Roles.Where(io=>io.Name!=null&&io.NormalizedName!=null)
             .Select(io=>new RoleModel
            {
               Name=io.Name,
               NormalizedName=io.NormalizedName,
            }).ToListAsync();
            return res;
        }
        #endregion

        #region GetAllUser
        public async Task<IEnumerable<UserModel>> GetAllUser()
        {
            var res =await userManager.Users.Select(io=>new UserModel
            {
                Name= io.Name,
                Email=io.Email,
                Surname=io.Surname,
                Password="***********",
                BirthDate=io.BirthDate,
                PersonalNumber=io.PersonalNumber,
                Username=io.UserName
            }).ToListAsync();

            return res;
        }
        #endregion
    }
}
