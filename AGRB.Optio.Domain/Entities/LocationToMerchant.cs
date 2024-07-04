using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("LocationToMerchants")]
    public class LocationToMerchant : AbstractEntity
    {
        [ForeignKey("Location")]
        public long LocationId { get; set; }

        [ForeignKey("Merchant")]
        public long MerchantId { get; set; }
        public virtual Location Location { get; set; }

        public virtual Merchant Merchant { get; set; }
    }
}
