using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("Feadbacks")]
    public class Feadback:AbstractEntity
    {
        [Column("User_Feadback")]
        public  required string FeadBack { get; set; }

        [DataType(DataType.Date)]
        public DateTime FeadbackDate { get; set; }

        [Column("Name_Of_User")]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Column("Rating_By_User")]
        public int RatingGivedByUser { get; set; }

        [Column("Feadback_Status")]
        public bool Status { get; set; } = false;

        [ForeignKey("User")]
        public required string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
