using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Optio.Core.Entities
{
    [Table("CategoryOfTransactions")]
    [Index(nameof(TransactionCategory),IsDescending =new bool[]{true})]
    public class Category:AbstractClass
    {

        [Column("Transaction_Category")]
        [StringLength(50,ErrorMessage ="Such Transaction Cattegory is not valid",MinimumLength=4)]
        public required string TransactionCategory { get; set; }
        public bool IsActive { get; set; } = true;

        [ForeignKey("typeOfTransaction")]
        public long TransactionTypeID { get; set; }

        public TypeOfTransaction typeOfTransaction { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }
    }
}
