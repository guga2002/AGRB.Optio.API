using System.ComponentModel.DataAnnotations;

namespace RGBA.Optio.Domain.Validation.VallidationAttributes
{
    public class DatatimeValidate: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var dateTime = value as DateTime? ?? new DateTime(2030, 01, 01);
            return dateTime <= DateTime.Now && dateTime >= new DateTime(1900, 01, 01);
        }
    }
}
