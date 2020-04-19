using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IBaseService<T> : IBaseServiceReadOnly<T>
    {
        Task<long> PostAsync(T entity);

        Task<bool> PutAsync(T entity);

        Task<bool> DeleteAsync(long id);

        Task<bool> DeleteAsync();
    }
}
