using AGRB.Optio.Application.Interfaces.InterfacesForTransaction;
using AGRB.Optio.Application.Models;

namespace AGRB.Optio.Application.Interfaces
{
    public interface ITransactionRelatedService : IAddInfo<ChannelModel>, IAddInfo<CategoryModel>, IAddInfo<TransactionTypeModel>,
        IGetInfo<ChannelModel, long>, IGetInfo<CategoryModel, long>, IGetInfo<TransactionTypeModel, long>,
        IRemoveInfo<ChannelModel, long>, IRemoveInfo<CategoryModel, long>, IRemoveInfo<TransactionTypeModel, long>,
        IUpdateInfo<ChannelModel, long>, IUpdateInfo<CategoryModel, long>, IUpdateInfo<TransactionTypeModel, long>
    {
    }
}
