using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RGBA.Optio.Core.Entities;
using RGBA.Optio.Domain.Custom_Exceptions;
using RGBA.Optio.Domain.Interfaces;
using RGBA.Optio.Domain.Models;
using RGBA.Optio.Domain.Models.RequestModels;
using RGBA.Optio.Domain.Services.Outer_Services;
using System.Net.Http.Headers;
using AGRB.Optio.API.StaticFiles;
using RGBA.Optio.Domain.Responses;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController(
        IAdminPanelService se,
        ILogger<CustomerController> log,
        SmtpService smtp,
        UserManager<User> userManager)
        : ControllerBase
    {

        [HttpGet]
        [Route(nameof(GetEmailVerificationMessage))]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<Response<IActionResult>> GetEmailVerificationMessage([FromQuery] string? securitySchema)
        {
            try
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
            catch (Exception exp)
            {
                return Response < IActionResult >.Ok(Content($"Error: {exp.Message}", "text/html"));
            }
        }

        [HttpPost]
        [Route(nameof(SignIn))]
        [AllowAnonymous]
        public async Task<Response<(SignInResult,string)>> SignIn([FromBody]SignInModel mod)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    throw new OptioGeneralException(mod.Username);
                }
                var res = await se.SignInAsync(mod);
                return Response<(SignInResult,string)>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<(SignInResult, string)>.Error(ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route(nameof(Registration))]
        [AllowAnonymous]
        public async Task<Response<IdentityResult>> Registration([FromBody]UserModel user)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(user.Username);
                }
                var res = await se.RegisterUserAsync(user,user.Password);
                return Response<IdentityResult>.Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<IdentityResult>.Error(ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Response<bool>> RefreshToken([FromQuery] string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(token);
                }
                if (User.Identity is { Name: not null, IsAuthenticated: true })
                {
                    var res = await se.RefreshToken(User.Identity.Name,token);
                    return Response<bool>.Ok(res);
                }
                else
                {
                    return Response<bool>.Error(ErrorKeys.BadRequest);
                }
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<bool>.Error(ErrorKeys.BadRequest);
            }
        }

        [HttpGet]
        [Route(nameof(ResetPasswordNow))]
        [AllowAnonymous]
        public async Task<Response<string>> ResetPasswordNow(string email,string newPassword)
        {
            try
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
            catch (Exception exp)
            {
                log.LogCritical(exp.Message,exp.StackTrace);
                return Response<string>.Error(exp.Message,exp.Message);
            }
        }


        [HttpGet]
        [Route(nameof(ForgetPassword))]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<Response<IActionResult>> ForgetPassword([FromQuery] string email,[FromQuery]string password)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(email);
                }
                var res = await se.ForgetPassword(email,password);
                return Response<IActionResult>.Ok(Content(res ? "<html><body><h1>Password Reset Successfully!</h1></body></html>" : "<html><body><h1>Password Reset Failed</h1></body></html>", "text/html"));
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<IActionResult>.Error(ErrorKeys.InternalServerError, exp.Message);
            }
        }


        [HttpPost]
        [Route(nameof(ResetPassword))]
        public async Task<Response<IdentityResult>> ResetPassword([FromBody] PasswordResetModel arg)
        {
            try
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
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<IdentityResult>.Error(exp.Message, exp.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route(nameof(Info))]
        public async Task<Response<UserModel>> Info()
        {
            try
            {
                if (User?.Identity is not { Name: not null, IsAuthenticated: true })
                    return Response<UserModel>.Error(ErrorKeys.BadRequest);
                var res = await se.Info(User.Identity.Name);
                return Response<UserModel>.Ok(res);

            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<UserModel>.Error(exp.Message,exp.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route(nameof(ConfirmEmail))]
        public async Task<Response<string>> ConfirmEmail()
        {
            try
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
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return Response<string>.Error(exp.Message,exp.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route(nameof(SignOutNow))]
        public async Task<Response<bool>> SignOutNow()
        {
            try
            {
                if (User.Identity is null || !User.Identity.IsAuthenticated || User.Identity.Name is null)
                    return Response<bool>.Error(ErrorKeys.BadRequest);
                var res=await se.SignOutAsync(User.Identity.Name);
                return Response<bool>.Ok(res);
            }
            catch (Exception exp)
            {
                return Response<bool>.Error(exp.Message,exp.StackTrace,ErrorKeys.BadRequest);
            }
        }
    }

}
