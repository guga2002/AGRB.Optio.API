namespace RGBA.Optio.Domain.Interfaces.InterfacesForTransaction
{
    public interface IGetInfo<T,K> where T : class
    {
        Task<T> GetByIdAsync(K id,T Identify);

        Task<IEnumerable<T>> GetAllAsync(T Identify);

        Task<IEnumerable<T>> GetAllActiveAsync(T Identify);
    }
}
