using Optio.Core.Interfaces;
using RGBA.Optio.Core.Entities;

namespace RGBA.Optio.Core.Interfaces
{
    public interface IValuteCourse:ICrudRepo<ValuteCourse, long>
    {
        Task<IEnumerable<ValuteCourse>> GetAllActiveValuteAsync();
    }
}
