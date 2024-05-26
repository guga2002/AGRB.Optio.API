using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models
{
    public class locationModel
    {
        [Required(ErrorMessage ="Location Name is Required!")]
        [StringLength(50,ErrorMessage ="Location Name is not Valid!",MinimumLength =2)]
        [Display(Name ="Location , Address")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Location Name should contain only letters, numbers, and underscores.")]
        public required string LocationName { get; set; }
    }
}
