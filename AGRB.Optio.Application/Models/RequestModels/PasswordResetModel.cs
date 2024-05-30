using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models.RequestModels
{
    public class PasswordResetModel
    {
        [Required(ErrorMessage = "Old Password is required")]
        public required string oldPassword { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public required string newPassword { get; set; }
    }
}
