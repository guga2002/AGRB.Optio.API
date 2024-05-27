using Microsoft.EntityFrameworkCore;
using Optio.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGBA.Optio.Core.Entities
{
    [Table("ExchangeRates")]
    [Index(nameof(Date),IsDescending = [true])]
    [Index(nameof(Rate),IsDescending = [true])]
    public class ExchangeRate:AbstractClass
    {
        [Column("Rate")]
        public decimal Rate { get; set; }

        [Column("Last_Updated")]
        public DateTime Date { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId {  get; set; }

        [Column("Status")]
        public bool IsActive { get; set; } = true;

        public virtual required Currency Currency { get; set; }
    }
}
