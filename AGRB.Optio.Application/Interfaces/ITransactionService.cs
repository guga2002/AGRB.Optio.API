using AGRB.Optio.Application.Interfaces.InterfacesForTransaction;
using AGRB.Optio.Application.Models;

namespace AGRB.Optio.Application.Interfaces
{
    public interface ITransactionService : IAddInfo<TransactionModel>,
     IUpdateInfo<TransactionModel, long>, IGetInfo<TransactionModel, long>, IRemoveInfo<TransactionModel, long>
    {

    }
}
