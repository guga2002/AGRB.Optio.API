using Microsoft.EntityFrameworkCore;
using Optio.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGBA.Optio.Core.Entities
{
    [Table("Currencies")]
    [Index(nameof(NameOfCurrency),IsDescending = [true])]
    [Index(nameof(CurrencyCode),IsDescending = [true])]
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Column("Name_Of_Currency")]
        [StringLength(30,ErrorMessage ="No such a currency exist",MinimumLength =2)]
        [Unicode(false)]
        public required string NameOfCurrency { get; set; }

        [Column("Currency_Code")]
        [StringLength(30, ErrorMessage = "Currency code is not valid", MinimumLength = 2)]
        [Unicode(false)]
        public required string CurrencyCode {  get; set; }

        [Column("Status_Of_Currency")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<Transaction> Transactions { get; set; }

        public virtual IEnumerable<ExchangeRate> Courses { get; set; }

    }
}
