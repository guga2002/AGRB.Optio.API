using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("Merchants")]
    [Index(nameof(Name), IsDescending = [true])]
    public class Merchant : AbstractEntity
    {
        [Column("Merchant_Name")]
        public required string Name { get; set; }

        [Column("Merchant_Status")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<LocationToMerchant> Locations { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }
        public Merchant()
        {
            Locations = new List<LocationToMerchant>();
            Transactions = new List<Transaction>();
        }
    }
}
