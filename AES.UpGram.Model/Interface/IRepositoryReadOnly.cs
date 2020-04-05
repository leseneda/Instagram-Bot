using System.Collections.Generic;
using System.Threading.Tasks;
using UpSocial.UpGram.Domain.Entity;

namespace UpSocial.UpGram.Domain.Interface
{
    public interface IRepositoryReadOnly<T> where T : BaseEntity
    {
        Task<T> SelectAsync(int id);

        Task<IEnumerable<T>> SelectAllAsync();
    }
}
