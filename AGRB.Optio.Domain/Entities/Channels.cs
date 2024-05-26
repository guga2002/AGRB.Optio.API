using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Optio.Core.Entities
{
    [Table("Channels")]
    [Index(nameof(ChannelType),IsDescending =new bool[] {true})]
    public class Channels:AbstractClass
    {
        [Column("Channel_Type")]
        [StringLength(50,ErrorMessage ="Such  a  CHanell Name  is not Valid",MinimumLength =2)]
        public required string ChannelType { get; set; }

        public bool IsActive { get; set; }=true;

        public virtual IEnumerable<Transaction> Transactions { get; set;}

        public Channels()
        {
            IsActive = true;
        }

    }
}
