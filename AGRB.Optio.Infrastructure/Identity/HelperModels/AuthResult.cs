namespace AGRB.Optio.Infrastructure.Identity.HelperModels
{
    public class AuthResult
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}
