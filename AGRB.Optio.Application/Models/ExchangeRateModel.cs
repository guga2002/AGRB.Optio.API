using RGBA.Optio.Domain.Validation.VallidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Models
{
    public class ExchangeRateModel
    {
        [Required(ErrorMessage = "Exchange rate is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Exchange rate must be a positive number.")]
        public decimal ExchangeRate { get; set; }

        [Required(ErrorMessage = "Date of exchange rate is required.")]
        [DataType(DataType.Date)]
        [DataTimeValidate]
        public DateTime DateOfExchangeRate { get; set; }

        [Required(ErrorMessage = "Currency ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Currency ID must be a positive integer.")]
        public int CurrencyId { get; set; }
    }
}
