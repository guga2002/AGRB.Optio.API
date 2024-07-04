using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface ILocationRepo : ICrudRepo<Location, long>
    {
        Task<IEnumerable<Location>> GetAllActiveLocationAsync();
    }
}
