using Microsoft.EntityFrameworkCore;
using Optio.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGBA.Optio.Core.Entities
{
    [Table("ValutesCourses")]
    [Index(nameof(DateOfValuteCourse),IsDescending =new bool[] { true })]
    [Index(nameof(ExchangeRate),IsDescending =new bool[] {true})]
    public class ValuteCourse:AbstractClass
    {
        [Column("Exchange_Rate")]
        public decimal ExchangeRate { get; set; }

        [Column("Last_Updated")]
        public DateTime DateOfValuteCourse { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyID {  get; set; }

        [Column("Status_Of_Valute")]
        public bool IsActive { get; set; } = true;

        public virtual Currency Currency { get; set; }
    }
}
