using System.ComponentModel.DataAnnotations;

namespace AGRB.Optio.Application.Models.RequestModels
{
    public class RoleModel
    {
        [Required(ErrorMessage = "the field is required")]
        public required string Name { get; set; }


        [Required(ErrorMessage = "the field is required")]
        public required string NormalizedName { get; set; }
    }
}
