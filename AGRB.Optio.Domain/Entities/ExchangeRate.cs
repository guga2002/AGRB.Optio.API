using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("ExchangeRates")]
    [Index(nameof(Date), IsDescending = [true])]
    [Index(nameof(Rate), IsDescending = [true])]
    public class ExchangeRate : AbstractEntity
    {
        [Column("Rate")]
        public decimal Rate { get; set; }

        [Column("Last_Updated")]
        public DateTime Date { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }

        [Column("Exchange_Rate_Status")]
        public bool IsActive { get; set; } = true;

        public virtual required Currency Currency { get; set; }
    }
}
