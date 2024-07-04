using System.ComponentModel.DataAnnotations;
namespace AGRB.Optio.Domain.Entities
{
    public abstract class AbstractEntity
    {
        [Key]
        public virtual long Id { get; set; }

    }
}
