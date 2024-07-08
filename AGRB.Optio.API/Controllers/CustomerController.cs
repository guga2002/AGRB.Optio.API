using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AGRB.Optio.Domain.Entities;
using AGRB.Optio.Application.Responses;
using AGRB.Optio.Application.Models;
using AGRB.Optio.Application.Models.RequestModels;
using AGRB.Optio.Application.Interfaces;
using AGRB.Optio.Domain.Services.Outer_Services;
using AGRB.Optio.Domain.Custom_Exceptions;
using AGRB.Optio.Application.StaticFiles;
using AGRB.Optio.Infrastructure.Identity.HelperModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RGBA.Optio.UI.Controllers
{
    /// <summary>
    /// Controller for Customer Related Actions
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController: ControllerBase
    {


        private readonly IAdminPanelService se;
        private readonly SmtpService smtp;
        private readonly UserManager<User> userManager;

        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// <param name="se">The admin panel service.</param>
        /// <param name="smtp">The Smtp Service for send email.</param>
        /// <param name="userManager">The UserManager Class.</param>
        public CustomerController(IAdminPanelService se, SmtpService smtp, UserManager<User> userManager)
        {
            this.se = se;
            this.smtp = smtp;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get Email Verifiaction V1.0
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        public async Task<Response<IActionResult>> GetEmailVerificationMessage([FromQuery] string? securitySchema)
        {
            if (User?.Identity is not null && securitySchema is not null && User.Identity.Name is not null)
            {
                var res = await se.ConfirmMail(User.Identity.Name, securitySchema);
                return Response<IActionResult>.Ok(
                    Content(
                    res
                        ? "<div style='text-align: center;'><h1 style='color: green; font-weight: bold; font-size: 24px;'>Congratulations!</h1><p style='font-size: 16px;'>Your email has been verified successfully.</p></div>"
                        : "<h1>somethings strange</h1>", "text/html"));
            }

            return Response<IActionResult>.Ok(Content(
                "<div style='text-align: center;'><h1 style='color: red; font-weight: bold; font-size: 24px;'>The link has expired!</h1><p style='font-size: 16px;'>Please contact support for assistance.</p></div>",
                "text/html"));
        }

        /// <summary>
        /// User Sign In service V2.0
        /// </summary>
        /// <returns>A response containing a SignInresult </returns>
        /// <remarks>
        ///  avalible for **Anymous member**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        [MapToApiVersion("2.0")]
        public async Task<Response<AuthResult>> SignIn([FromBody] SignInModel mod)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(mod.Username);
            }
            var res = await se.SignInAsync(mod);
            if (res is not null)
            {
                return Response<AuthResult>.Ok(res);
            }
            return Response<AuthResult>.Error("Sign in  wailed");
        }

        /// <summary>
        /// User Registration Service V2.0
        /// </summary>
        /// <returns>A response containing IdentityResult</returns>
        /// <remarks>
        ///  avalible for **Anymous**
        /// </remarks>
        [HttpPost]
        [Route(nameof(Registration))]
        [AllowAnonymous]
        [MapToApiVersion("2.0")]
        public async Task<Response<AuthResult>> Registration([FromBody] UserModel user)
        {

            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(user.Username);
            }
            var res = await se.RegisterUserAsync(user, user.Password);
            return Response<AuthResult>.Ok(res);
        }

        /// <summary>
        ///Refresh Authorization Token V1.0
        /// </summary>
        /// <returns>A response containing a boolean</returns>
        /// <remarks>
        ///  avalible for **Authorize User**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [MapToApiVersion("1.0")]
        public async Task<Response<AuthResult>> RefreshToken([FromBody]TokenRequest req)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(req.ToString());
            }
            if (User.Identity is { Name: not null, IsAuthenticated: true })
            {
                var res = await se.RefreshToken(req);
                return Response<AuthResult>.Ok(res);
            }
            else
            {
                return Response<AuthResult>.Error(ErrorKeys.BadRequest);
            }
        }

        /// <summary>
        /// Reset Password ( if forget) V2.0
        /// </summary>
        /// <returns>A response containing a string</returns>
        /// <remarks>
        ///  avalible for **anymous**
        /// </remarks>
        [HttpGet]
        [Route(nameof(ResetPasswordNow))]
        [AllowAnonymous]
        [MapToApiVersion("2.0")]
        public async Task<Response<string>> ResetPasswordNow(string email, string newPassword)
        {
            if (!await se.IsUserExist(email))
            {
                return Response<string>.Error(ErrorKeys.NoUser);
            }
            var link = Url.ActionLink(nameof(ForgetPassword), "Customer", new { Email = email, Password = newPassword }, Request.Scheme);
            if (link is null) return Response<string>.Ok(ErrorKeys.BadRequest);
            var body = $@"
                  <div align='center' style='font-family: Arial, sans-serif;'>
                  <p style='font-size: 16px;'>გადადი ლინკზე რათა შეცვალო პაროლი:</p>
                 <p style='font-size: 16px;'>
                 <a href='{link}' style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 5px;'>შეცვალე პაროლი
                 </a>
                 </p>
                 <p style='font-size: 16px;'>ლინკი ვალიდურია 24 საათის განავლობაში</p>
                 <p style='font-size: 16px;'>ჩვენი ჯგუფი გიხდის მადლობას..</p>
                  <h2 style='font-size: 16px;color:red;'>თუ თქვენ  არ გამოგიგზავნიათ მოთხოვნა, გთხოვთ დაგვიკავშირდეთ!</h2>
                </div>";
            smtp.SendMessage(email, "პაროლის შეცვლის მოთხოვნა" + '_' + DateTime.Now.Hour + ':' + DateTime.Now.Minute, body);
            return Response<string>.Ok(SuccessKeys.EmailSent);
        }

        /// <summary>
        /// Endpoint for  call server V1.0
        /// </summary>
        /// <returns>A response containing a Actionresult.</returns>
        /// <remarks>
        ///  avalible for **Anymous member**
        /// </remarks>
        [HttpGet]
        [Route(nameof(ForgetPassword))]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        public async Task<Response<IActionResult>> ForgetPassword([FromQuery] string email, [FromQuery] string password)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(email);
            }
            var res = await se.ForgetPassword(email, password);
            return Response<IActionResult>.Ok(Content(res ? "<html><body><h1>Password Reset Successfully!</h1></body></html>" : "<html><body><h1>Password Reset Failed</h1></body></html>", "text/html"));
        }


        /// <summary>
        /// Reset Password (When you remember old password) V1.0
        /// </summary>
        /// <returns>A response containing a Identity Result</returns>
        /// <remarks>
        ///  avalible for **Authorize user**
        /// </remarks>
        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route(nameof(ResetPassword))]
        public async Task<Response<IdentityResult>> ResetPassword([FromBody] PasswordResetModel arg)
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException(arg.NewPassword);
            }

            if (User.Identity is not { Name: not null, IsAuthenticated: true })
                return Response<IdentityResult>.Error(ErrorKeys.BadRequest);

            var res = await se.ResetPasswordAsync(arg, User.Identity.Name);
            return Response<IdentityResult>.Ok(res);
        }

        /// <summary>
        /// Get Information About current User V1.0
        /// </summary>
        /// <returns>A response containing a user Model</returns>
        /// <remarks>
        ///  avalible for **Authorize User**
        /// </remarks>
       [MapToApiVersion("1.0")]
        [HttpGet]
        [Route(nameof(Info))]
        public async Task<Response<UserModel>> Info()
        {
            if (User?.Identity is not { Name: not null, IsAuthenticated: true })
                return Response<UserModel>.Error(ErrorKeys.BadRequest);
            var res = await se.Info(User.Identity.Name);
            return Response<UserModel>.Ok(res);
        }

        /// <summary>
        /// Confirm Email Address V2.0
        /// </summary>
        /// <returns>A response containing a string.</returns>
        /// <remarks>
        ///  avalible for **Authorize User**
        /// </remarks>
        [HttpGet]
        [Route(nameof(ConfirmEmail))]
        [MapToApiVersion("2.0")]
        public async Task<Response<string>> ConfirmEmail()
        {
            if (!ModelState.IsValid)
            {
                throw new OptioGeneralException("info");
            }

            if (User.Identity is not null && User.Identity.Name != null && User.Identity.IsAuthenticated)
            {
                if (await se.IsEmailConfirmed(User.Identity.Name))
                {
                    throw new ArgumentException(ErrorKeys.AlreadyVerified);
                }

                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return Response<string>.Error(ErrorKeys.BadRequest);

                var rek = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.ActionLink(nameof(GetEmailVerificationMessage), "Customer",
                    new { SecuritySchema = rek }, Request.Scheme);
                if (link == null) return Response<string>.Error(ErrorKeys.NotFound);
                await se.SendLinkToUser(User.Identity.Name, link);
                return Response<string>.Ok(link);
            }
            return Response<string>.Error(ErrorKeys.BadRequest);
        }

        /// <summary>
        /// Sign out current user V2.0
        /// </summary> 
        /// <returns>A response containing boolean </returns>
        /// <remarks>
        ///  avalible for **Authorize user**
        /// </remarks>
        [HttpPost]
        [Route("[action]")]
        [MapToApiVersion("2.0")]
        public async Task<Response<bool>> SignOutNow()
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated || User.Identity.Name is null)
                return Response<bool>.Error(ErrorKeys.BadRequest);
            var res = await se.SignOutAsync(User.Identity.Name);
            return Response<bool>.Ok(res);
        }
    }

}
