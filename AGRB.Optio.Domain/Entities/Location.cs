using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("Locations")]
    [Index(nameof(LocationName), IsDescending = [true])]
    public class Location : AbstractEntity
    {
        [Column("Location_Name")]
        public required string LocationName { get; set; }

        [Column("Location_Status")]
        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<LocationToMerchant> Merchants { get; set; }

        public Location()
        {
            Merchants = new List<LocationToMerchant>();
        }
    }
}
