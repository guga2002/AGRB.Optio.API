using RGBA.Optio.Domain.Interfaces.InterfacesForTransaction;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Interfaces
{
    public interface ITransactionService:IAddInfo<TransactionModel>, 
     IUpdateInfo<TransactionModel,long>,IGetInfo<TransactionModel,long>,IRemoveInfo<TransactionModel,long>
    {
      
    }
}
