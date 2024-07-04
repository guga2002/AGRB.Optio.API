using AGRB.Optio.Domain.Entities;

namespace AGRB.Optio.Domain.Interfaces
{
    public interface ICategoryRepo : ICrudRepo<Category, long>
    {
        Task<IEnumerable<Category>> GetAllActiveAsync();
    }
}
