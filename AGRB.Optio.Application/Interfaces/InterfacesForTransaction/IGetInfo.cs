namespace RGBA.Optio.Domain.Interfaces.InterfacesForTransaction
{
    public interface IGetInfo<T,K> where T : class
    {
        Task<T> GetByIdAsync(K id,T identify);

        Task<IEnumerable<T>> GetAllAsync(T identify);

        Task<IEnumerable<T>> GetAllActiveAsync(T identify);
    }
}
