using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("RefreshTokens")]
    public class RefreshToken
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public string? JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public User User { get; set; }
    }
}
