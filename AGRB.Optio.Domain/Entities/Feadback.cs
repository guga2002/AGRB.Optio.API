using Optio.Core.Entities;
using RGBA.Optio.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("Feadbacks")]
    public class Feadback:AbstractClass
    {
        public  required string FeadBack { get; set; }

        [DataType(DataType.Date)]
        public DateTime FeadbackDate { get; set; }

        [Column("Name_Of_User")]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Range(0,100)]
        public int RatingGivedByUser { get; set; }

        public bool Status { get; set; } = false;

        [ForeignKey("user")]
        public string UserId { get; set; }

        public virtual User user { get; set; }
    }
}
