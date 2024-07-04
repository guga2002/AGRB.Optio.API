using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface IMerchantRepo : ICrudRepo<Merchant, long>
    {
        Task<IEnumerable<Merchant>> GetAllActiveMerchantAsync();
        Task<bool> AssignLocationToMerchant(long merchantId, long locationId);
        Task<List<Transaction>> GetAllTransactions();
    }
}
