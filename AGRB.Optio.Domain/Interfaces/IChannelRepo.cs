using Optio.Core.Entities;

namespace Optio.Core.Interfaces
{
    public interface IChannelRepo : ICrudRepo<Channels,long>
    {
        Task<IEnumerable<Channels>> GetAllActiveChannelAsync();
    }
}
