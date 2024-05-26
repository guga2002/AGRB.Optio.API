using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Optio.Core.Entities
{
    [Table("TypeOfTransactions")]
    [Index(nameof(TransactionName),IsDescending = new bool[] { true })]
    public class TypeOfTransaction:AbstractClass
    {
        [Column("Transaction_Name")]
        [StringLength(100,ErrorMessage ="Transaction name is not valid!!",MinimumLength =3)]
        public required string TransactionName { get; set; }

        [Column("Status_Of_Transaction_Type")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<Category> Category { get; set; }
    }
}
