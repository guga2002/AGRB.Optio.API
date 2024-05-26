using Microsoft.EntityFrameworkCore;
using RGBA.Optio.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Optio.Core.Entities
{
    [Table("Locations")]
    [Index(nameof(LocationName),IsDescending =new bool[] {true})]
    public class Location:AbstractClass
    {
        [Column("Location_Name")]
        [StringLength(50,ErrorMessage ="Location name is not valid",MinimumLength =3)]
        public required string LocationName {  get; set; }

        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<LocationToMerchant> Merchants { get; set; }
    }
}
