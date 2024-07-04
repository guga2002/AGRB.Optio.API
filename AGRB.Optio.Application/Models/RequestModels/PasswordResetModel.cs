using System.ComponentModel.DataAnnotations;

namespace AGRB.Optio.Application.Models.RequestModels
{
    public class PasswordResetModel
    {
        [Required(ErrorMessage = "Old Password is required")]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string NewPassword { get; set; }
    }
}
