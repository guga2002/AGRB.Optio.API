namespace RGBA.Optio.Domain.Interfaces.InterfacesForTransaction
{
    public interface IRemoveInfo<T,K> where T : class
    {
        Task<bool> RemoveAsync(K id,T identity);

        Task<bool> SoftDeleteAsync(K id,T Identify);
    }
}
