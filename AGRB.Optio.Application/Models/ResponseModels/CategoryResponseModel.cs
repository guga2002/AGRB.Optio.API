namespace AGRB.Optio.Application.Models.ResponseModels
{
    public class CategoryResponseModel
    {
        public string? TransactionCategory { get; set; }
        public decimal Average { get; set; }
        public decimal TransactionCount { get; set; }
        public decimal TransactionVolume { get; set; }
    }
}
