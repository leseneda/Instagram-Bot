using System.Collections.Generic;
using System.Threading.Tasks;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IBaseService<T> : IBaseServiceReadOnly<T>
    {
        Task<long> PostAsync(T entity);

        Task<bool> PutAsync(T entity);

        Task<bool> DeleteAsync(int id);

        Task<bool> DeleteAsync();
    }
}
