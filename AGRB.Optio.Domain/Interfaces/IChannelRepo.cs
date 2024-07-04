using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface IChannelRepo : ICrudRepo<Channels, long>
    {
        Task<IEnumerable<Channels>> GetAllActiveChannelAsync();
    }
}
