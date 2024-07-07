using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AGRB.Optio.Domain.Data;
using AGRB.Optio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AGRB.Optio.Infrastructure.Identity.HelperModels;

namespace AGRB.Optio.Application.Interfaces.Identity
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration conf;
        private readonly OptioDB _context;
        private readonly UserManager<User> UserManage;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public JwtService(OptioDB context, TokenValidationParameters tokenValidationParameters, IConfiguration conf, UserManager<User> userManage)
        {
            this.conf = conf;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
            UserManage = userManage;
        }

        public async Task<AuthResult> GenerateToken(string username)
        {
            var user = await UserManage.FindByNameAsync(username) ??
               throw new UnauthorizedAccessException("Access is unauthorized");
            var role = await UserManage.GetRolesAsync(user);
            var claims = new[]
            {
            new Claim(ClaimTypes.GivenName,user.Name),
                new Claim(ClaimTypes.Name,user.UserName??"Default"),
                    new Claim(ClaimTypes.Surname,user.Surname),
                        new Claim(ClaimTypes.DateOfBirth,user.BirthDate.ToShortDateString()),
                            new Claim(ClaimTypes.HomePhone,user.PhoneNumber??"Default"),
                                new Claim(ClaimTypes.Email,user.Email??"Default"),
                    new Claim(ClaimTypes.Role,role.FirstOrDefault()??"NotDefined"),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(conf.GetSection("JWT:Secret").Value ?? throw new ArgumentException("value  of  key  is null")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(50),
                signingCredentials: credentials);

            string JWTToken = new JwtSecurityTokenHandler().WriteToken(token);

            RefreshToken refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMonths(1),
                Token = GetRandomString() + Guid.NewGuid(),
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();



            return new AuthResult()
            {
                Token = JWTToken,
                RefreshToken = refreshToken.Token,
                Success = true,

            };
        }

        public async Task<RefreshTokenResponse> VerifyToken(TokenRequest tokenRequest)
        {
            JwtSecurityTokenHandler? jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                RefreshToken? storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenRequest.RefreshToken);
                if (storedToken == null)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>{
                     "token does not found"
                    }
                    };
                }
                ClaimsPrincipal? tokenVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken); 

                var jti = tokenVerification.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>{
                     "token doesn't match"
                    }
                    };
                }

                //////////////////
                long utcExpireDate = long.Parse(tokenVerification.Claims.FirstOrDefault(d => d.Type == JwtRegisteredClaimNames.Exp).Value);

                // UTC to DateTime
                DateTime expireDate = UTCtoDateTime(utcExpireDate);

                Console.WriteLine($"expireDate: {expireDate} - now: {DateTime.UtcNow}");

                if (expireDate > DateTime.UtcNow)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>{
                        "Token not expired"
                    }
                    };
                }

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    bool result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);//?

                    if (!result)
                    {
                        return null;
                    }
                }
                if (storedToken.IsUsed)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>{
                     "token used."
                    }
                    };
                }
                if (storedToken.IsRevoked)
                {
                    return new RefreshTokenResponse()
                    {
                        Success = false,
                        Errors = new List<string>{
                     "token revoked."
                    }
                    };
                }

                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                return new RefreshTokenResponse()
                {
                    UserId = storedToken.UserId,
                    Success = true,
                };
            }
            catch (Exception e)
            {

                return new RefreshTokenResponse()
                {
                    Errors = new List<string>{
                    e.Message
                },
                    Success = false
                };
            }



        }

        private DateTime UTCtoDateTime(long unixTimeStamp)
        {
            var datetimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            datetimeVal = datetimeVal.AddSeconds(unixTimeStamp).ToLocalTime();

            return datetimeVal;
        }


        private string GetRandomString()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPRSTUVYZWX0123456789";
            return new string(Enumerable.Repeat(chars, 35).Select(n => n[new Random().Next(n.Length)]).ToArray());

        }
    }
}
