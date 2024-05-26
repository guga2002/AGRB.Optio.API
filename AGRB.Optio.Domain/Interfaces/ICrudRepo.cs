namespace Optio.Core.Interfaces
{
    public interface ICrudRepo<T,K> where T : class
    {
        Task<long>AddAsync(T entity);   
        Task<bool> RemoveAsync(T entity);
        Task<bool> UpdateAsync(K id,T entity);
        Task<bool> SoftDeleteAsync(K id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(K id);
    }
}
