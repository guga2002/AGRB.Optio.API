using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("Currencies")]
    [Index(nameof(NameOfCurrency), IsDescending = [true])]
    [Index(nameof(CurrencyCode), IsDescending = [true])]
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Column("Name_Of_Currency")]
        [Unicode(false)]
        public required string NameOfCurrency { get; set; }

        [Column("Currency_Code")]
        [Unicode(false)]
        public required string CurrencyCode { get; set; }

        [Column("Status_Of_Currency")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<Transaction> Transactions { get; set; }

        public virtual IEnumerable<ExchangeRate> Courses { get; set; }

        public Currency()
        {
            Transactions = new List<Transaction>();
            Courses = new List<ExchangeRate>();
        }
    }
}
