using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models
{
    public class ChanellModel
    {
        [Required(ErrorMessage = "Channel Name is required.")]
        [StringLength(50, ErrorMessage = "Channel Name is not valid", MinimumLength = 3)]
        [Display(Name = "Transaction Channel Type")]
        [RegularExpression(@"^[a-zA-Z\u10D0-\u10F6\s]*$", ErrorMessage = "Transaction Type should contain only letters (Latin or Georgian) and spaces.")]
        public required string ChannelType { get; set; }
    }
}
