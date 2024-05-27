using RGBA.Optio.Domain.Validation.VallidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RGBA.Optio.Domain.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name should contain only letters and spaces.")]
        [Display(Name = "Name of user")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Surname must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Surname should contain only letters and spaces.")]
        [Display(Name = "Surname of user")]
        public required string Surname { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [DatatimeValidate]
        [Display(Name = "Birthday of user")]
        public required DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Username should contain only letters, numbers, and underscores.")]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [StringLength(11, ErrorMessage = "Personal number must be 11 in length", MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal number should contain only 11 digits.")]
        [Display(Name = "Personal Number Of User")]
        public required string PersonalNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [NotMapped]
        public  required string Password { get; set; }
    }
}
