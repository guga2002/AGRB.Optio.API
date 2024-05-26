using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models
{
    public class CurrencyModel
    {
        [Required(ErrorMessage = "Name of currency is required.")]
        [StringLength(50, ErrorMessage = "Currency Name is not valid", MinimumLength = 3)]
        [Display(Name = "Name of Valute")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Valute Name should contain only letters and spaces.")]
        public required string NameOfValute { get; set; } 

        [Required(ErrorMessage = "Currency code is required.")]
        [StringLength(50, ErrorMessage = "Currency Code is not valid", MinimumLength = 3)]
        [Display(Name = "Currency code")]
        public required string CurrencyCode { get; set; }

      
    }
}
