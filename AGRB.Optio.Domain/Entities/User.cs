using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGRB.Optio.Domain.Entities
{
    [Table("Users")]
    [Index(nameof(PersonalNumber), IsDescending = [true])]
    public class User : IdentityUser
    {
        [Column("User_Name")]
        public string Name { get; set; }

        [Column("User_Surname")]
        public string Surname { get; set; }


        [Column("Personal_Number")]
        public string PersonalNumber { get; set; }


        [Column("User_BirthDay")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public virtual IEnumerable<Feadback> Feadbacks { get; set; }

        public virtual IEnumerable<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            Feadbacks = new List<Feadback>();
        }
    }
}
