using RGBA.Optio.Domain.Interfaces.InterfacesForTransaction;
using RGBA.Optio.Domain.Models;

namespace RGBA.Optio.Domain.Interfaces
{
    public interface ITransactionRelatedService:IAddInfo<ChanellModel>, IAddInfo<CategoryModel>, IAddInfo<TransactionTypeModel>,
        IGetInfo<ChanellModel,long>, IGetInfo<CategoryModel, long>, IGetInfo<TransactionTypeModel, long>,
        IRemoveInfo<ChanellModel, long>, IRemoveInfo<CategoryModel, long>, IRemoveInfo<TransactionTypeModel, long>,
        IUpdateInfo<ChanellModel, long>, IUpdateInfo<CategoryModel, long>, IUpdateInfo<TransactionTypeModel, long>
    {
    }
}
