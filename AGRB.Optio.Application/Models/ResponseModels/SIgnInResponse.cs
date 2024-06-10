namespace AGRB.Optio.Application.Models.ResponseModels
{
    public class SignInResponse
    {
        public string AuthToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ValidateTill { get; set; }
    }
}
