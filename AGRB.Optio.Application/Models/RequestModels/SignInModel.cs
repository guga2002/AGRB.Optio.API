using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models.RequestModels
{
    public class SignInModel
    {
        [Required(ErrorMessage ="the field is required")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "the field is required")]
        public required string Password { get; set; }

        public bool SetCookie { get; set; }
    }
}
