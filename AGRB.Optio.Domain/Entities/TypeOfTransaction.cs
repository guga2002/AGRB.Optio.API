using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("TypeOfTransactions")]
    [Index(nameof(TransactionName), IsDescending = [true])]
    public class TypeOfTransaction : AbstractEntity
    {
        [Column("Transaction_Name")]
        public required string TransactionName { get; set; }

        [Column("Status_Transaction_Type")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<Category> Category { get; set; }
        public TypeOfTransaction()
        {
            Category = new List<Category>();
        }
    }
}
