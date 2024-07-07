using AGRB.Optio.Infrastructure.Identity.HelperModels;

namespace AGRB.Optio.Application.Interfaces.Identity
{
    public interface IJwtService
    {
        Task<AuthResult> GenerateToken(string user);
        Task<RefreshTokenResponse> VerifyToken(TokenRequest tokenRequest);

    }
}
