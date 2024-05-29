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
        public async Task<IActionResult> GetEmailVerificationMessage([FromQuery] string? securitySchema)
        {
            try
            {
                if (User?.Identity is not null && securitySchema is not null && User.Identity.Name is not null)
                {
                    var res = await se.ConfirmMail(User.Identity.Name, securitySchema);
                    return Content(
                        res
                            ? "<div style='text-align: center;'><h1 style='color: green; font-weight: bold; font-size: 24px;'>Congratulations!</h1><p style='font-size: 16px;'>Your email has been verified successfully.</p></div>"
                            : "<h1>somethings strange</h1>", "text/html");
                }

                return Content(
                    "<div style='text-align: center;'><h1 style='color: red; font-weight: bold; font-size: 24px;'>The link has expired!</h1><p style='font-size: 16px;'>Please contact support for assistance.</p></div>",
                    "text/html");
            }
            catch (Exception exp)
            {
                return Content($"Error: {exp.Message}", "text/html");
            }
        }

        [HttpPost]
        [Route(nameof(SignIn))]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody]SignInModel mod)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    throw new OptioGeneralException(mod.Username);
                }
                var res = await se.SignInAsync(mod);
                return Ok(res.Item2);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route(nameof(Registration))]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromBody]UserModel user)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(user.Username);
                }
                var res = await se.RegisterUserAsync(user,user.Password);
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RefreshToken([FromQuery] string token)
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
                    return Ok(res);
                }
                else
                {
                    return Unauthorized(new AuthenticationHeaderValue("Bearer"));
                }
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route(nameof(ResetPasswordNow))]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordNow(string email,string newPassword)
        {
            try
            {
                if (!await se.IsUserExist(email))
                {
                    return BadRequest(ErrorKeys.NoUser);
                }
                var link = Url.ActionLink(nameof(ForgetPassword), "Customer", new { Email = email, Password = newPassword }, Request.Scheme);
                if (link is null) return BadRequest(ErrorKeys.BadRequest);
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
                return Ok(SuccessKeys.EmailSent);

            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message,exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpGet]
        [Route(nameof(ForgetPassword))]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromQuery] string email,[FromQuery]string password)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(email);
                }
                var res = await se.ForgetPassword(email,password);
                return Content(res ? "<html><body><h1>Password Reset Successfully!</h1></body></html>" : "<html><body><h1>Password Reset Failed</h1></body></html>", "text/html");
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }


        [HttpPost]
        [Route(nameof(ResetPassword))]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetModel arg)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(arg.NewPassword);
                }

                if (User.Identity is not { Name: not null, IsAuthenticated: true })
                    return Unauthorized(new AuthenticationHeaderValue("Bearer"));

                var res = await se.ResetPasswordAsync(arg, User.Identity.Name);
                return Ok(res);
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route(nameof(Info))]
        public async Task<IActionResult> Info()
        {
            try
            {
                if (User?.Identity is not { Name: not null, IsAuthenticated: true })
                    return Unauthorized(new AuthenticationHeaderValue("Bearer"));
                var res = await se.Info(User.Identity.Name);
                return Ok(res);

            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return StatusCode(503, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route(nameof(ConfirmEmail))]
        public async Task<IActionResult> ConfirmEmail()
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
                    if (user == null) return BadRequest(ErrorKeys.BadRequest);

                    var rek = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var link = Url.ActionLink(nameof(GetEmailVerificationMessage), "Customer",
                        new { SecuritySchema = rek }, Request.Scheme);
                    if (link == null) return Unauthorized(new AuthenticationHeaderValue("Bearer"));
                    await se.SendLinkToUser(User.Identity.Name, link);
                    return Ok(link);
                }

                return Unauthorized(new AuthenticationHeaderValue("Bearer"));
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return BadRequest(exp.Message);
            }
        }

        [HttpPost]
        [Route(nameof(SignOutNow))]
        public async Task<IActionResult> SignOutNow()
        {
            try
            {
                if (User.Identity is null || !User.Identity.IsAuthenticated || User.Identity.Name is null)
                    return Unauthorized(new AuthenticationHeaderValue("Bearer"));
                var res=await se.SignOutAsync(User.Identity.Name);
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorKeys.InternalServerError);
            }
        }
    }

}
