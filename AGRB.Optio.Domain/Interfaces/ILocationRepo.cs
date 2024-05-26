using Optio.Core.Entities;

namespace Optio.Core.Interfaces
{
    public interface ILocationRepo:ICrudRepo<Location, long>
    {
        Task<IEnumerable<Location>> GetAllActiveLocationAsync();
    }
}
