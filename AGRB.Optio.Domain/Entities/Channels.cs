using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("Channels")]
    [Index(nameof(ChannelType), IsDescending = [true])]
    public class Channels : AbstractEntity
    {
        [Column("Channel_Type")]
        public required string ChannelType { get; set; }

        [Column("Chanell_Status")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<Transaction> Transactions { get; set; }

        public Channels()
        {
            Transactions = new List<Transaction>();
        }
    }
}
