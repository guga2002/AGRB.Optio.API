
using AGRB.Optio.Application.Validation.VallidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace AGRB.Optio.Application.Models
{
    public class ExchangeRateModel
    {
        [Required(ErrorMessage = "Exchange rate is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Exchange rate must be a positive number.")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Date of exchange rate is required.")]
        [DataType(DataType.Date)]
        [DataTimeValidate]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Currency ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Currency ID must be a positive integer.")]
        public int CurrencyId { get; set; }
    }
}
