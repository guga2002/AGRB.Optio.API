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

namespace RGBA.Optio.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IAdminPanelService ser;
        private readonly ILogger<CustomerController> log;
        private readonly SmtpService smtp;
        private readonly UserManager<User> usermanager;
        public CustomerController(IAdminPanelService se, ILogger<CustomerController> log, SmtpService smtp, UserManager<User> usermanager)
        {
            this.ser = se;
            this.log = log;
            this.smtp = smtp;
            this.usermanager = usermanager;
        }

        [HttpGet]
        [Route("GetEmailVerification")]
        [ApiExplorerSettings(IgnoreApi = true)]// ar gamochndeba swaggershi
        [AllowAnonymous]
        public async Task<IActionResult> GetEmailVerificationMessage([FromQuery] string? SecuritySchema)
        {
            try
            {
                if (User is not null && User.Identity is not null && SecuritySchema is not null && User.Identity.Name is not null)
                {
                    var res = await ser.ConfirmMail(User.Identity.Name, SecuritySchema);
                    if (res)
                    {
                        return Content("<div style='text-align: center;'><h1 style='color: green; font-weight: bold; font-size: 24px;'>Congratulations!</h1><p style='font-size: 16px;'>Your email has been verified successfully.</p></div>", "text/html");

                    }
                    return Content("<h1>somethings strange</h1>", "text/html");
                }
                else
                {

                    return Content("<div style='text-align: center;'><h1 style='color: red; font-weight: bold; font-size: 24px;'>The link has expired!</h1><p style='font-size: 16px;'>Please contact support for assistance.</p></div>", "text/html");
                }
            }
            catch (Exception exp)
            {
                return Content($"Error: {exp.Message}", "text/html");
            }
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody]SignInModel mod)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    throw new OptioGeneralException(mod.Username);
                }
                var res = await ser.SignInAsync(mod);
                if (res.Item2 is not null)
                {
                    return Ok(res.Item2);
                }
                return BadRequest();
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return StatusCode(503, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromBody]UserModel user)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(user.Username);
                }
                var res = await ser.RegisterUserAsync(user,user.Password);
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
        [ApiExplorerSettings(IgnoreApi = true)]// ar gamochndeba swaggershi
        public async Task<IActionResult> RefreshToken([FromQuery] string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(token);
                }
                if (User.Identity is not null && User.Identity.Name is not null&&User.Identity.IsAuthenticated)
                {
                    var res = await ser.RefreshToken(User.Identity.Name,token);
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
                return StatusCode(503, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordNow(string email,string NewPassword)
        {
            try
            {
                if (!await ser.IsUserExist(email))
                {
                    return BadRequest("no such User Exist,  FIrst  You Must Reister Please");
                }
                var link = Url.ActionLink("ForgetPassword", "Customer", new { Email = email, Password = NewPassword }, Request.Scheme);
                if (link is not null)
                {
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
                    return Ok("Reset link is sent to your email");
                }
                return BadRequest("bad request");

            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message,exp.StackTrace);
                return BadRequest(exp.Message);
            }
        }


        [HttpGet]
        [Route("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)] //endpointi ar minda ro gamochndes swaggershi
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromQuery] string Email,[FromQuery]string Password)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(Email);
                }
                var res = await ser.ForgetPassword(Email,Password);
                if (res)
                {
                    return Content("<html><body><h1>Password Reset Successfully!</h1></body></html>", "text/html");
                }
                else
                {
                    return Content("<html><body><h1>Password Reset Failed</h1></body></html>", "text/html");
                }
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
                return StatusCode(503, "Internal Server Error");
            }
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetModel arg)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException(arg.newPassword);
                }
                if (User is not null&&User.Identity!=null&&User.Identity.Name!=null &&User.Identity.IsAuthenticated)
                {
                    var res = await ser.ResetPasswordAsync(arg, User.Identity.Name);
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
                return StatusCode(503, "Internal Server Errror");
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Info()
        {
            try
            {
                if (User.Identity is not null&&User.Identity.Name is not null&&User.Identity.IsAuthenticated)
                {
                    var res = await ser.Info(User.Identity.Name);
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
                return StatusCode(503, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ConfirmEmail()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new OptioGeneralException("info");
                }
                if (User.Identity is not null&&User.Identity.Name!=null&&User.Identity.IsAuthenticated)
                {
                    if (await ser.isEmailConfirmed(User.Identity.Name))
                    {
                        throw new ArgumentException("Email is already  Verified");
                    }
                    var user = await usermanager.FindByNameAsync(User.Identity.Name);
                    if (user != null) {

                       var rek= await usermanager.GenerateEmailConfirmationTokenAsync(user);
                        var link = Url.ActionLink("GetEmailVerificationMessage", "Customer", new { SecuritySchema = rek}, Request.Scheme);
                        await ser.sendlinktouser(User.Identity.Name, link);
                        return Ok(link);
                    }
                    return BadRequest("somethings is bad");
                }
                else
                {
                    return Unauthorized(new AuthenticationHeaderValue("Bearer"));
                }
            }
            catch (Exception exp)
            {
                log.LogCritical(exp.Message);
               return BadRequest(exp.Message);
            }

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Signout()
        {
            try
            {
                if(User.Identity is not null&&User.Identity.IsAuthenticated&&User.Identity.Name is not null)
                {
                    var res=await ser.SignOutAsync(User.Identity.Name);
                    return Ok(res);
                }
                return Unauthorized(new AuthenticationHeaderValue("Bearer"));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
