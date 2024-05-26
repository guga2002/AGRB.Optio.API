using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "Category  Name is required.")]
        [StringLength(50, ErrorMessage = "Category Name is not valid", MinimumLength = 3)]
        [Display(Name = "Transaction category name")]
        [RegularExpression(@"^[a-zA-Z\u10D0-\u10F6\s]*$", ErrorMessage = "Transaction Type should contain only letters (Latin or Georgian) and spaces.")]
        public required string TransactionCategory { get; set; }

        public required long TransactionTypeID { get; set; }
    }
}
