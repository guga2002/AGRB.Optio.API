using Optio.Core.Entities;

namespace Optio.Core.Interfaces
{
    public interface ICategoryRepo:ICrudRepo<Category,long>
    {
        Task<IEnumerable<Category>> GetAllActiveAsync();
    }
}
