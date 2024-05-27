using Optio.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGBA.Optio.Core.Entities
{
    [Table("LocationToMerchants")]
    public class LocationToMerchant:AbstractClass
    {
        [ForeignKey("location")]
        public long LocationId { get; set; }

        [ForeignKey("merchant")]
        public long MerchantId { get; set; }
        public Location Location { get; set; }

        public Merchant Merchant { get; set; }
    }
}
