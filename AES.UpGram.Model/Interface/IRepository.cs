using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IRepository<T> : IRepositoryReadOnly<T> where T : BaseEntity
    {
        Task<long> InsertAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> RemoveAsync(int id);

        Task<bool> RemoveAsync();
    }
}
