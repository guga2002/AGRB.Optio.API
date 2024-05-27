using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models
{
    public class MerchantModel
    {

        [Required(ErrorMessage ="Merchant Name is Required!")]
        [StringLength(50,ErrorMessage ="merchant Name is not valid!",MinimumLength =2)]
        [Display(Name="Merchant")]
        public  required string Name { get; set; }
    }
}
