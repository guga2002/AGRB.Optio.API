namespace AGRB.Optio.Application.Interfaces.InterfacesForTransaction
{
    public interface IAddInfo<T> where T : class
    {
        Task<long> AddAsync(T entity);
    }
}
