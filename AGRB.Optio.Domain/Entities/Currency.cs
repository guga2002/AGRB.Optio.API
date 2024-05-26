using Microsoft.EntityFrameworkCore;
using Optio.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGBA.Optio.Core.Entities
{
    [Table("Curencies")]
    [Index(nameof(NameOfValute),IsDescending =new bool[]{true})]
    [Index(nameof(CurrencyCode),IsDescending =new bool[] { true })]
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Column("Name_Of_Valute")]
        [StringLength(30,ErrorMessage ="No such a valute exist",MinimumLength =2)]
        [Unicode(false)]
        public required string NameOfValute { get; set; }

        [Column("Currency_Code")]
        [StringLength(30, ErrorMessage = "Curency code is not valid", MinimumLength = 2)]
        [Unicode(false)]
        public required string CurrencyCode {  get; set; }

        [Column("Status_Of_Currency")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<Transaction> Transactions { get; set; }

        public virtual IEnumerable<ValuteCourse> Courses { get; set; }

    }
}
