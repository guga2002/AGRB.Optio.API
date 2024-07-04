using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("CategoryOfTransactions")]
    [Index(nameof(TransactionCategory), IsDescending = [true])]
    public class Category : AbstractEntity
    {
        [Column("Transaction_Category")]
        public required string TransactionCategory { get; set; }

        [Column("Category_Status")]
        public bool IsActive { get; set; } = true;

        [ForeignKey("TypeOfTransaction")]
        public long TransactionTypeId { get; set; }

        public virtual TypeOfTransaction TypeOfTransaction { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }

        public Category()
        {
            Transactions = new List<Transaction>();
        }
    }
}
