using System.Threading.Tasks;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IRepository<T> : IRepositoryReadOnly<T> where T : BaseEntity
    {
        Task<long> InsertAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> RemoveAsync(long id);

        Task<bool> RemoveAsync();
    }
}
