using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Optio.Core.Entities
{
    [Table("CategoryOfTransactions")]
    [Index(nameof(TransactionCategory), IsDescending = [true])]
    public class Category:AbstractClass
    {

        [Column("Transaction_Category")]
        [StringLength(50,ErrorMessage ="Such Transaction category is not valid",MinimumLength=4)]
        public required string TransactionCategory { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey("typeOfTransaction")]
        public long TransactionTypeId { get; set; }

        public TypeOfTransaction TypeOfTransaction { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }
    }
}
