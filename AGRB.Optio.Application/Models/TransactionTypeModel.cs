using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models
{
    public class TransactionTypeModel
    {
        [Required(ErrorMessage ="Trabsaction Type Name Is Required")]
        [StringLength(30,ErrorMessage ="Name is not valid",MinimumLength =3)]
        [Display(Name ="Transaction Type")]
        [RegularExpression(@"^[a-zA-Z\u10D0-\u10F6\s]*$", ErrorMessage = "Transaction Type should contain only letters (Latin or Georgian) and spaces.")]
        public required string TransactionName { get; set; }
   
    }
}
