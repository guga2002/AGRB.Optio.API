using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AGRB.Optio.Application.Models.RequestModels
{
    public class FeadbackModel
    {
        [StringLength(100,ErrorMessage ="no correct  format")]
        public required string FeadBack { get; set; }

        [DataType(DataType.Date)]
        public DateTime FeadbackDate { get; set; }

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Range(0, 100)]
        public int RatingGivedByUser { get; set; }


        public required string UserId { get; set; }
    }
}
