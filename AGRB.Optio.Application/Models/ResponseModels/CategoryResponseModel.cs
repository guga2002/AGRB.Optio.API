
namespace RGBA.Optio.Domain.Models.ResponseModels
{
    public class CategoryResponseModel
    {
        public  string? TransactionCategory { get; set; }
        public  long TransactionTypeId { get; set; }
        public decimal TransactionCount {  get; set; }
        public decimal TransactionVolume {  get; set; }
    }
}
