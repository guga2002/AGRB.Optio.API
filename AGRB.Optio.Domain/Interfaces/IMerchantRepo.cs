using Optio.Core.Entities;

namespace Optio.Core.Interfaces
{
    public interface IMerchantRepo:ICrudRepo<Merchant, long>
    {
        Task<IEnumerable<Merchant>> GetAllActiveMerchantAsync();
        Task<bool> AssignLocationtoMerchant(long Merchantid, long Locationid);
        Task<List<Transaction>> getalltransactions();
    }
}
