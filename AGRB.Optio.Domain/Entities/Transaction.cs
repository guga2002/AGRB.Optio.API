using Microsoft.EntityFrameworkCore;
using RGBA.Optio.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Optio.Core.Entities
{
    [Table("Transactions")]
    [Index(nameof(Amount),IsDescending =new bool[] { true })]
    [Index(nameof(AmountEquivalent), IsDescending = new bool[] { true })]
    [Index(nameof(Date), IsDescending = new bool[] { true })]
    public class Transaction:AbstractClass
    {
        [Column("Date_Of_Transaction")]
        public DateTime Date { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }

        [Column("Amount_Equivalent")]
        public decimal AmountEquivalent { get; set; }

        [Column("Transaction_Status")]
        public bool IsActive { get; set; } = true;

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }


        [ForeignKey("Category")]
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }


        [ForeignKey("Merchant")]
        public long MerchantId { get; set; }
        public virtual Merchant Merchant {  get; set; }


        [ForeignKey("Channel")]
        public long ChannelId { get; set; }
        public virtual Channels Channel { get; set; }

    }
}
