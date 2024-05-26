using System.ComponentModel.DataAnnotations;
namespace Optio.Core.Entities
{
    public abstract class AbstractClass
    {
        [Key]
        public long Id { get; set; }
     
    }
}
