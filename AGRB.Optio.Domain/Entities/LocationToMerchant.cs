using Optio.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace RGBA.Optio.Core.Entities
{
    [Table("LocationToMerchants")]
    public class LocationToMerchant:AbstractClass
    {
        [ForeignKey("location")]
        public long LocatrionId { get; set; }

        [ForeignKey("merchant")]
        public long merchantId { get; set; }
        public Location location { get; set; }

        public Merchant merchant { get; set; }
    }
}
